using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace SkyEye.Data.Positions
{
    public class EurekaPyros
    {
        private readonly List<EurekaFate> Fates = new();

        public static readonly (int, EurekaWeather)[] Weathers = new (int, EurekaWeather)[]
        {
            (10, EurekaWeather.FairSkies),
            (28, EurekaWeather.HeatWaves),
            (46, EurekaWeather.Thunder),
            (64, EurekaWeather.Blizzards),
            (82, EurekaWeather.UmbralWind),
            (100, EurekaWeather.Snow)
        };

        public static Dictionary<int, string> deadFateDic = new Dictionary<int, string>() { { 1388, "-1" }, { 1389, "-1" }, { 1390, "-1" }, { 1391, "-1" }, { 1392, "-1" }, { 1393, "-1" }, { 1394, "-1" }, { 1395, "-1" }, { 1396, "-1" }, { 1397, "-1" }, { 1398, "-1" }, { 1399, "-1" }, { 1400, "-1" }, { 1401, "-1" }, { 1402, "-1" }, { 1403, "-1" }, { 1404, "-1" }, { 1407, "-1" }, { 1408, "-1" } };

    public static readonly List<Vector3> ElementalPositions = new()
        {
            new(-22.74618f, 768.31287f, 510.34402f),
            new(-33.69695f, 683.36426f, 243.2133f),
            new(-110.5325f, 764.73047f, 617.9566f),
            new(-124.4339f, 674.73566f, 305.94412f),
            new(-153.78471f, 666.87354f, -199.7227f),
            new(-154.0888f, 764.3937f, 520.66394f),
            new(-231.1558f, 658.3021f, -558.73425f),
            new(-248.9779f, 658.02454f, -678.9961f),
            new(-332.1558f, 641.8102f, 622.6001f),
            new(-358.3534f, 661.66284f, 343.216f),
            new(-398.83002f, 666.6412f, -672.0005f),
            new(-425.1501f, 659.0271f, 447.99982f),
            new(-439.0166f, 675.91943f, -206.9271f),
            new(-480.7868f, 673.946f, -345.3124f),
            new(-596.1195f, 674.65735f, -288.56842f),
            new(36.22116f, 676.982f, -201.40761f),
            new(59.199173f, 754.18854f, 702.04144f),
            new(134.1277f, 675.81714f, -721.8558f),
            new(145.9791f, 679.15015f, -401.46198f),
            new(181.1693f, 659.8738f, -278.7961f),
            new(183.26439f, 751.2846f, 837.4545f),
            new(197.4814f, 717.4583f, 299.7896f),
            new(233.8187f, 681.5016f, -584.3393f),
            new(265.5905f, 745.7182f, 758.2044f),
            new(272.4338f, 723.1208f, 163.47511f),
            new(311.00122f, 738.55225f, 510.64432f),
            new(351.5322f, 677.0166f, -669.42975f),
            new(387.78052f, 737.80646f, 566.47375f),
            new(423.8157f,  661.35266f, -212.1115f),
            new(444.6063f, 723.1206f, 360.91608f),
            new(460.0268f, 724.2298f, 482.7586f),
            new(462.5268f, 665.4831f, -601.6767f),
            new(472.0545f, 670.66797f, -332.1753f),
            new(501.59842f, 668.1235f, -467.8756f),
            new(732.08276f, 656.574f, -343.12262f)
        };

        public EurekaPyros(List<EurekaFate> fates) { Fates = fates; }

        public static List<EurekaFate> PyrosFates = new()
            {
                new(1388, 38, 795, 484,     "Medias Res", "Leucosia", "惨叫", new Vector2(320.9842f, 201.438f), "Pyros Bhoot", new Vector2(27.0f, 26.0f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Water, EurekaElement.Ice, true, 35),
                new(1389, 39, 795, 484,     "High Voltage","Flauros", "雷兽", new Vector2(386.33926f, 358.24274f), "Thunderstorm Sprite", new Vector2(29.0f, 29.0f), EurekaWeather.Thunder, EurekaWeather.Thunder, EurekaElement.Lightning, EurekaElement.Lightning, false, 36),
                new(1390, 40, 795, 484,     "On the Nonexistent", "The Sophist", "诡辩者", new Vector2(513.146f, 468.17218f), "Pyros Apanda", new Vector2(32.1f, 31.5f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Wind, EurekaElement.Earth, false, 37),
                new(1391, 41, 795, 484,     "Creepy Doll", "Graffiacane", "塔塔露", new Vector2(94.2181f, 765.32275f), "Valking", new Vector2(23.0f, 37.0f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Ice, EurekaElement.Lightning, false, 38),
                new(1392, 42, 795, 484,     "Quiet, Please", "Askalaphos", "阿福", new Vector2(-112.91578f, 342.06964f), "Overdue Tome", new Vector2(19.2f, 29.2f), EurekaWeather.UmbralWind, EurekaWeather.None, EurekaElement.Wind, EurekaElement.Earth, false, 39),
                new(1393, 43, 795, 484,     "Up and Batym", "Grand Duke Batym", "大公", new Vector2(-176.88625f, -394.7388f), "Dark Troubadour", new Vector2(17.8f, 14.1f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Earth, EurekaElement.Earth, true, 40),
                new(1394, 44, 795, 484,     "Rondo Aetolus", "Aetolus", "雷鸟", new Vector2(-578.14294f, -386.20517f), "Islandhander", new Vector2(10.1f, 14.2f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Lightning, EurekaElement.Wind, false, 41),
                new(1395, 45, 795, 484,     "Scorchpion King", "Lesath", "蝎子", new Vector2(-433.2961f, -543.60785f), "Bird Eater", new Vector2(12.6f, 11.1f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Fire, EurekaElement.Wind, false, 42),
                new(1396, 46, 795, 484,     "Burning Hunger", "Eldthurs", "火巨人", new Vector2(-315.6653f, -738.8786f), "Pyros Crab", new Vector2(15.2f, 6.4f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Fire, EurekaElement.Fire, false, 43),
                new(1397, 47, 795, 484,     "Dry Iris", "Iris", "海燕", new Vector2(-12.539706f, -486.02472f), "Northern Swallow", new Vector2(21.3f, 12.2f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Water, EurekaElement.Water, false, 44),
                new(1398, 48, 795, 484,     "Thirty Whacks", "Lamebrix Strikebocks", "哥布林", new Vector2(24.877388f, -666.72644f), "Illuminati Escapee", new Vector2(21.8f, 8.4f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Earth, EurekaElement.Lightning, false, 45),
                new(1399, 49, 795, 484,     "Put Up Your Dux", "Dux", "雷军", new Vector2(291.65533f, -631.25104f), "Matanga Castaway", new Vector2(27.4f, 8.9f), EurekaWeather.Thunder, EurekaWeather.Thunder, EurekaElement.Lightning, EurekaElement.Fire, false, 46),
                new(1400, 50, 795, 484,     "You Do Know Jack", "Lumber Jack", "树人", new Vector2(429.29465f, -496.1107f), "Pyros Treant", new Vector2(30.2f, 11.4f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Earth, EurekaElement.Lightning, false, 47),
                new(1401, 51, 795, 484,     "Mister Bright-eyes", "Glaukopis", "明眸", new Vector2(543.06006f, -316.8035f), "Val Skatene", new Vector2(32.0f, 15.2f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Fire, EurekaElement.Wind, false, 48),
                new(1402, 52, 795, 484,     "Haunter of the Dark", "Ying-Yang", "阴阳", new Vector2(-491.60144f, 632.4779f), "Pyros Hecteyes", new Vector2(11.5f, 34.3f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Water, EurekaElement.Water, false, 49),
                new(1403, 53, 795, 484,     "Heavens' Warg", "Skoll", "狼", new Vector2(126.20996f, 413.7807f), "Pyros Shuck", new Vector2(16.0f, 36.8f), EurekaWeather.Blizzards, EurekaWeather.None, EurekaElement.Ice, EurekaElement.Earth, false, 50),
                new(1404, 54, 795, 484,     "Lost Epic", "Penthesilea", "彭女士", new Vector2(720.3792f, -780.43463f), "Val Bloodglider", new Vector2(33.6f, 8.2f), EurekaWeather.HeatWaves, EurekaWeather.None, EurekaElement.Fire, EurekaElement.Fire, false, 50),
                new(1407, null, 795, 484,   "We're All Mad Here", "Bunny Fate 1", "小兔子", new Vector2(144.02539f, 214.77539f), null, Vector2.Zero, EurekaWeather.None, EurekaWeather.None, new EurekaElement(), new EurekaElement(), false, 35, false, true),
                new(1408, null, 795, 484,   "Uncommon Nonsense", "Bunny Fate 2", "大兔子", new Vector2(172.51315f, -524.5715f), null, Vector2.Zero, EurekaWeather.None, EurekaWeather.None, new EurekaElement(), new EurekaElement(), false, 46, false, true)
            };

        public List<EurekaFate> GetFates() => Fates;

        public static List<EurekaWeather> GetZoneWeathers() => Weathers.Select(x => x.Item2).ToList();

        public (EurekaWeather Weather, TimeSpan Timeleft) GetCurrentWeatherInfo() => EorzeaWeather.GetCurrentWeatherInfo(Weathers);

        public static List<DateTime> GetWeatherForecast(EurekaWeather targetWeather, int count) =>
            EorzeaWeather.GetCountWeatherForecasts(targetWeather, count, Weathers);

        public List<(EurekaWeather Weather, TimeSpan Time)> GetAllNextWeatherTime() => EorzeaWeather.GetAllWeathers(Weathers);

        public static (DateTime Start, DateTime End) GetWeatherUptime(EurekaWeather targetWeather, DateTime start)
            => EorzeaWeather.GetWeatherUptime(targetWeather, Weathers, start);

        public void SetPopTimes(Dictionary<ushort, long> keyValuePairs)
        {
            var zoneFates = Fates.Where(x => x.IncludeInTracker).ToList();
            foreach (var fate in zoneFates)
            {
                if (keyValuePairs.ContainsKey((ushort)fate.TrackerId))
                    fate.SetKill(keyValuePairs[(ushort)fate.TrackerId]);
                else
                    fate.ResetKill();
            }
        }
    }
}
