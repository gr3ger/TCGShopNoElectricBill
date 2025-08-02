using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TCGNoElectricBills;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(typeof(Plugin));
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CPlayerData), "UpdateBill")]
    static void UpdateBill(EBillType billType, int dueDayChange, float amountToPayChange)
    {
        if(billType == EBillType.Electric)
            ZeroElectricBill();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CPlayerData), "SetBill")]
    static void SetBill(EBillType billType, int dayPassed, float amountToPay)
    {
        if(billType == EBillType.Electric)
            ZeroElectricBill();
    }

    public static void ZeroElectricBill()
    {
        Logger.LogInfo("Zeroing out Electric Bill");
        var bill = CPlayerData.GetBill(EBillType.Electric);
        bill.amountToPay = 0;
    }
}