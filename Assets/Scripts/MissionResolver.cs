using UnityEngine;
using System.Collections.Generic;

// Static helper: no need to attach it to any GameObject.
public static class MissionResolver
{
    public struct MissionResult
    {
        public bool isSuccess;
        public float finalSuccessChance;
        public float finalReward;
        public string resultLog;
        public List<EmployeeData> casualties; // List of deceased employees
    }

    // True if the ghost hides because the team is too big (e.g. Shade)
    public static bool ExceedsPersonnelLimit(MissionData mission, List<EmployeeData> team)
    {
        return mission != null
            && mission.ghost != null
            && mission.ghost.hasPersonnelLimit
            && team != null
            && team.Count > mission.ghost.maxAllowedPersonnel;
    }

    // Skill used for this mission type, reduced by stress
    private static float GetEffectiveSkill(EmployeeData emp, MissionType type)
    {
        float skill = 0f;
        switch (type)
        {
            case MissionType.Investigation: skill = emp.investigationSkill; break;
            case MissionType.Exorcism: skill = emp.exorcismSkill; break;
            case MissionType.Filming: skill = emp.recordingSkill; break;
        }

        if (emp.stressLevel >= 81f) skill *= 0.6f;       // -40% efficiency
        else if (emp.stressLevel >= 41f) skill *= 0.8f;  // -20% efficiency

        return skill;
    }

    // THE one formula. Used both for the UI prediction and the real roll.
    public static float CalculateChance(MissionData mission, List<EmployeeData> team)
    {
        if (mission == null || team == null || team.Count == 0) return 0f;
        if (ExceedsPersonnelLimit(mission, team)) return 0f;

        float teamPower = 0f;
        foreach (var emp in team)
        {
            if (emp == null) continue;
            teamPower += GetEffectiveSkill(emp, mission.type);
        }

        float equipmentBonus = mission.assignedEquipment != null ? mission.assignedEquipment.successChanceBonus : 0f;
        float locDifficulty = mission.location != null ? mission.location.difficultyScore : 10f;
        float ghostDifficulty = mission.ghost != null ? mission.ghost.baseDifficulty : 10f;

        float chance = (teamPower + equipmentBonus) - (locDifficulty + ghostDifficulty) + 50f;
        return Mathf.Clamp(chance, 5f, 95f);
    }

    public static MissionResult ResolveMission(MissionData mission, List<EmployeeData> assignedEmployees)
    {
        MissionResult result = new MissionResult();
        result.casualties = new List<EmployeeData>();

        if (mission == null)
        {
            result.resultLog = "Error: Mission data is null.";
            return result;
        }

        if (assignedEmployees == null || assignedEmployees.Count == 0)
        {
            result.resultLog = "Error: No team assigned.";
            return result;
        }

        // 1. Ghost personnel limit check
        if (ExceedsPersonnelLimit(mission, assignedEmployees))
        {
            result.isSuccess = false;
            result.finalSuccessChance = 0f;
            result.finalReward = 0f;
            result.resultLog = $"Failed! The {mission.ghost.ghostName} vanished because the team was too large.";
            return result;
        }

        // 2. Success roll (shared formula)
        result.finalSuccessChance = CalculateChance(mission, assignedEmployees);
        float roll = Random.Range(0f, 100f);
        result.isSuccess = roll <= result.finalSuccessChance;

        float safetyBonus = mission.assignedEquipment != null ? mission.assignedEquipment.safetyBonus : 0f;

        // 3. Rewards, stress gain, casualty checks
        if (result.isSuccess)
        {
            // GetTotalReward() already includes the equipment multiplier
            result.finalReward = mission.GetTotalReward();
            result.resultLog = $"Success! Mission completed. Reward: ${result.finalReward}";

            foreach (var emp in assignedEmployees)
            {
                if (emp == null) continue;
                emp.stressLevel = Mathf.Clamp(emp.stressLevel + 15f, 0f, 100f);
            }

            if (HQManager.Instance != null) HQManager.Instance.ModifyReputation(2f);
        }
        else
        {
            result.finalReward = 0f;
            result.resultLog = "Mission failed. The team retreated under heavy pressure.";

            foreach (var emp in assignedEmployees)
            {
                if (emp == null) continue;
                emp.stressLevel = Mathf.Clamp(emp.stressLevel + 35f, 0f, 100f);
            }

            float baseAggression = mission.ghost != null ? mission.ghost.aggressionLevel : 15f;

            foreach (var emp in assignedEmployees)
            {
                if (emp == null) continue;

                float stressVulnerability = emp.stressLevel > 60f ? 1.5f : 1.0f;
                float deathRisk = (baseAggression * stressVulnerability) - safetyBonus;
                float deathRoll = Random.Range(0f, 100f);

                if (deathRoll <= Mathf.Max(deathRisk, 0f))
                {
                    result.casualties.Add(emp);
                }
            }

            if (result.casualties.Count > 0)
            {
                result.resultLog += $" Catastrophe! {result.casualties.Count} employee(s) lost in action.";
                if (HQManager.Instance != null) HQManager.Instance.ModifyReputation(-4f);
            }
            else
            {
                if (HQManager.Instance != null) HQManager.Instance.ModifyReputation(-1f);
            }
        }

        // 4. Remove deceased employees from HQ roster
        if (HQManager.Instance != null)
        {
            foreach (var deadEmp in result.casualties)
            {
                HQManager.Instance.hiredEmployees.Remove(deadEmp);
            }
        }

        return result;
    }
}