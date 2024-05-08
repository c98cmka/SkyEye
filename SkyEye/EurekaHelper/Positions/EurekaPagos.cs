using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace SkyEye.Data.Positions
{
    public class EurekaPagos
    {
        private readonly List<EurekaFate> Fates = new();

        public static readonly (int, EurekaWeather)[] Weathers = new (int, EurekaWeather)[]
        {
            (10, EurekaWeather.FairSkies),
            (28, EurekaWeather.Fog),
            (46, EurekaWeather.HeatWaves),
            (64, EurekaWeather.Snow),
            (82, EurekaWeather.Thunder),
            (100, EurekaWeather.Blizzards)
        };

        public static Dictionary<int, string> deadFateDic = new Dictionary<int, string>() {{1351, "-1"}, {1369, "-1"}, {1353, "-1"}, {1354, "-1"}, {1355, "-1"}, {1366, "-1"}, {1357, "-1"}, {1356, "-1"}, {1352, "-1"}, {1360, "-1"}, {1358, "-1"}, {1361, "-1"}, {1362, "-1"}, {1359, "-1"}, {1363, "-1"}, {1365, "-1"}, {1364, "-1"}, {1367, "-1"}, {1368, "-1"}};

        public static readonly List<Vector3> ElementalPositions = new()
        {
            new(-41.5411f, -471.7219f, -365.66238f),
            new(-93.89765f, -740.93286f, 390.37912f),
            new(-141.2347f, -562.60614f, -282.6532f),
            new(-127.999695f, -546.0727f, -101.5866f),
            new(-181.1255f, -733.01514f, 431.18948f),
            new(-218.84619f, -720.4173f, 187.913f),
            new(-224.1831f, -606.68243f, -133.2441f),
            new(-263.0846f, -569.75006f, -337.29272f),
            new(-270.0375f, -644.00146f, 14.772899f),
            new(-286.91522f, -587.29865f, -223.6939f),
            new(-286.1163f, -677.2271f, 385.7882f),
            new(-339.81677f, -671.70374f, 160.0002f),
            new(-432.67923f, -588.9239f, -539.0688f),
            new(-460.07782f, -703.45056f, 260.35468f),
            new(-467.4751f, -656.8985f, -26.89501f),
            new(-479.99982f, -700.4795f, 387.8571f),
            new(-493.17822f, -611.9679f, -50.352207f),
            new(-572.7231f, -618.6403f, -167.888f),
            new(-596.4171f, -620.23083f, -455.2062f),
            new(-627.7185f, -697.3568f, 147.9566f),
            new(-677.4056f, -649.40497f, 42.32301f),
            new(-713.46967f, -624.1894f, -270.3222f),
            new(0.9566075f, -721.777f, 210.29039f),
            new(8.574106f, -738.0508f, 323.6507f),
            new(20.43179f, -546.3381f, 6.304727f),
            new(87.11651f, -510.49722f, -159.9996f),
            new(114.5379f, -632.4933f, 119.393f),
            new(152.0979f, -495.13928f, -282.6871f),
            new(192.1393f, -647.50696f, 197.5168f),
            new(273.51492f, -532.7138f, -128.0004f),
            new(286.3525f, -641.0692f, 25.18766f),
            new(337.38052f, -721.52655f, 291.58188f),
            new(354.9172f, -688.889f, 105.6524f),
            new(426.56082f, -578.40295f, -73.76903f),
            new(450.2676f, -747.31274f, 217.9324f),
            new(462.04968f, -553.8097f, -225.24661f),
            new(489.01028f, -740.3993f, 424.49872f),
            new(528.40967f, -688.61096f, 50.57122f),
            new(714.3189f, -630.0619f, -321.2806f),
            new(735.9999f, -629.8334f, -274.5996f)
        };

        public EurekaPagos(List<EurekaFate> fates) { Fates = fates; }

        public static List<EurekaFate> PagosFates = new()
        {
            new(1351, 21, 763, 467,     "Eternity", "The Snow Queen", "周冬雨", new Vector2(-0.1946f, 234.4f), "Yukinko", new Vector2(22.0f, 27.2f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Ice, EurekaElement.Ice, false, 20),
            new(1369, 22, 763, 467,     "Cairn Blight 451", "Taxim", "读书人", new Vector2(194.67f, 340.845f), "Demon of the Incunable", new Vector2(25.0f, 28.0f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Earth, EurekaElement.Wind, true, 21),
            new(1353, 23, 763, 467,     "Ash the Magic Dragon", "Ash Dragon", "灰烬龙", new Vector2(428.54f, 425.388f), "Blood Demon", new Vector2(30.1f, 29.8f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Fire, EurekaElement.Fire, false, 22),
            new(1354, 24, 763, 467,     "Conqueror Worm", "Glavoid", "魔虫", new Vector2(559.886f, 320.959f), "Val Worm", new Vector2(32.8f, 27.3f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Earth, EurekaElement.Earth, false, 23),
            new(1355, 25, 763, 467,     "Melting Point", "Anapos", "安娜波", new Vector2(575.078f, 10.7835f), "Snowmelt Sprite", new Vector2(32.9f, 21.5f), EurekaWeather.Fog, EurekaWeather.Fog, EurekaElement.Water, EurekaElement.Water, false, 24),
            new(1366, 26, 763, 467,     "The Wobbler in Darkness", "Hakutaku", "白泽", new Vector2(378.451f, 58.118f), "Blubber Eyes", new Vector2(28.9f, 22.3f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Fire, EurekaElement.Fire, false, 25),
            new(1357, 27, 763, 467,     "Does It Have to Be a Snowman", "King Igloo", "雪屋王", new Vector2(-222.164f, -269.87f), "Huwasi", new Vector2(17.2f, 16.2f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Ice, EurekaElement.Ice, false, 26),
            new(1356, 28, 763, 467,     "Disorder in the Court", "Asag", "阿萨格", new Vector2(-548.7f, -520.554f), "Wandering Opken", new Vector2(10.5f, 11.0f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Lightning, EurekaElement.Lightning, false, 27),
            new(1352, 29, 763, 467,     "Cows for Concern", "Surabhi", "山羊", new Vector2(-579.55f, -132.637f), "Pagos Billygoat", new Vector2(10.0f, 20.0f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Earth, EurekaElement.Earth, false, 28),
            new(1360, 30, 763, 467,     "Morte Arthro", "King Arthro", "螃蟹", new Vector2(-638.802f, -299.99f), "Val Snipper", new Vector2(8.7f, 15.4f), EurekaWeather.Fog, EurekaWeather.None, EurekaElement.Water, EurekaElement.Water, false, 29),
            new(1358, 31, 763, 467,     "Brothers", "Mindertaur/Eldertaur", "双牛", new Vector2(-380.98f, -131.9f), "Lab Minotaur", new Vector2(13.9f, 18.7f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Earth, EurekaElement.Wind, false, 30),
            new(1361, 32, 763, 467,     "Apocalypse Cow", "Holy Cow", "圣牛", new Vector2(252.05f, -227.33f), "Elder Buffalo", new Vector2(26.5f, 16.9f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Water, EurekaElement.Wind, false, 31),
            new(1362, 33, 763, 467,     "Third Impact", "Hadhayosh", "贝爷", new Vector2(467.758f, -150.738f), "Lesser Void Dragon", new Vector2(31.0f, 18.5f), EurekaWeather.Thunder, EurekaWeather.None, EurekaElement.Lightning, EurekaElement.Lightning, false, 32),
            new(1359, 34, 763, 467,     "Eye of Horus", "Horus", "荷鲁斯", new Vector2(217.191f, -58.8899f), "Void Vouivre", new Vector2(26.0f, 20.2f), EurekaWeather.HeatWaves, EurekaWeather.None, EurekaElement.Fire, EurekaElement.Fire, false, 33),
            new(1363, 35, 763, 467,     "Eye Scream for Ice Cream", "Arch Angra Mainyu", "大眼", new Vector2(118.55f, 178.48f), "Gawper", new Vector2(24.0f, 25.0f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Wind, EurekaElement.Wind, false, 34),
            new(1365, 36, 763, 467,     "Cassie and the Copycats", "Copycat Cassie", "凯西", new Vector2(37.76f, -361.25f), "Ameretat", new Vector2(21.0f, 14.5f), EurekaWeather.Blizzards, EurekaWeather.None, EurekaElement.Ice, EurekaElement.Ice, false, 35),
            new(1364, 37, 763, 467,     "Louhi on Ice", "Louhi", "娄希", new Vector2(727.415f, -126.398f), "Val Corpse", new Vector2(36.0f, 19.0f), EurekaWeather.None, EurekaWeather.None, EurekaElement.Ice, EurekaElement.Ice, true, 35),
            new(1367, null, 763, 467,   "Down the Rabbit Hole", "Bunny Fate 1", "小兔子", new Vector2(-163.29f, 300.203f), null, Vector2.Zero, EurekaWeather.None, EurekaWeather.None, new EurekaElement(), new EurekaElement(), false, 21, false, true),
            new(1368, null, 763, 467,   "Curiouser and Curiouser", "Bunny Fate 2", "大兔子", new Vector2(-46.6f, -9.67f), null, Vector2.Zero, EurekaWeather.None, EurekaWeather.None, new EurekaElement(), new EurekaElement(), false, 31, false, true)
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
