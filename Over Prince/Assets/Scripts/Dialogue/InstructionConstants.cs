using UnityEngine;

public static class InstructionConstants {
    public const float instructionFadeTime = 0.5f;
    public static Instruction ControlInstructions = new Instruction("Touch Screen to Explore", "Use Arrow Keys to Explore", "Use Left Stick to Explore");

    public static Instruction BasicAttackInstruction = new Instruction("Tap this Button to Attack", "Press Z to Attack", "Press A to Attack");
}

public class Instruction {
    public string mobile;
    public string pc;
    public string console;

    public Instruction(string mobile, string pc, string console) {
        this.mobile = mobile;
        this.pc = pc;
        this.console = console;
    }

    public string GetInstructionsBasedOnDevice() {
        return SystemInfo.deviceType switch
        {
            DeviceType.Handheld => mobile,
            DeviceType.Desktop => pc,
            DeviceType.Console => console,
            _ => pc,
        };
    }
}