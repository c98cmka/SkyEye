using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SkyEye.EurekaHelper.Positions
{
    public class RabbitTreasurePosition
    {
        public static readonly Dictionary<uint, List<Vector3>> RabbitTreasurePositions = new() {
        {
            763, [
                // Pagos
                new(-737.83690f, -677.9246f, 143.3176f),
                new(-651.98720f, -688.063f, 085.1839f),
                new(-606.53420f, -700.6216f, 143.02383f),
                new(-589.40370f, -698.3553f, 015.37505f),
                new(-430.18720f, -680.5923f, 060.50428f),
                new(-386.33823f, -677.493f, 468.21628f),
                new(-340.45220f, -671.272f, 425.2418f),
                new(-298.34700f, -664.6113f, 450.374f),
                new(-289.46200f, -585.3194f, -178.6195f),
                new(-285.17250f, -586.4724f, -245.163f),
                new(-283.38950f, -573.506f, -337.2024f),
                new(-259.90164f, -602.8868f, -139.11661f),
                new(-257.58450f, -627.9702f, -067.44543f),
                new(-222.82150f, -557.8751f, -370.6961f),
                new(-222.44050f, -628.8225f, -069.52717f),
                new(-199.55620f, -594.9023f, -154.88f),
                new(-175.56530f, -568.1696f, -237.659f),
                new(-158.79890f, -576.3018f, -200.3996f),
                new(-147.99794f, -563.5579f, -282.92133f),
                new(292.912100f, -640.7648f, 015.70983f),
                new(351.901800f, -690.0383f, 115.0498f),
                new(398.619700f, -678.8519f, -015.59305f),
                new(445.425400f, -741.8608f, 188.4324f),
                new(476.741000f, -739.2648f, 421.0621f),
                new(487.021100f, -756.6284f, 291.6779f),
                new(514.979100f, -702.1201f, 139.7072f),
                new(516.052000f, -679.7125f, -037.79275f),
                new(530.784900f, -720.4700f, 230.5152f),
                new(543.909000f, -730.7676f, 355.7726f),
                new(631.058800f, -730.9005f, 317.3849f),
                new(643.954300f, -702.8303f, 117.7415f)
            ]
        }, {
            795, [
                // Pyros
                new(-469.810200f, 659.1795f, 441.7094f),
                new(-464.448400f, 660.6446f, 419.2033f),
                new(-438.394400f, 660.7888f, 400.7463f),
                new(-340.102100f, 660.3159f, 384.9267f),
                new(-198.295300f, 757.9987f, 477.8044f),
                new(-197.162200f, 759.5239f, 599.0419f),
                new(-189.479080f, 671.6885f, 323.32883f),
                new(-150.433500f, 762.666f, 451.7729f),
                new(-105.592600f, 762.684f, 686.5082f),
                new(-038.891640f, 769.8203f, 504.8099f),
                new(-038.375523f, 675.38214f, 354.2082f),
                new(-011.633400f, 773.301f, 601.2862f),
                new(002.4903228f, 764.1788f, 411.15732f),
                new(032.1309000f, 754.25977f, 689.94055f),
                new(092.8179700f, 754.2609f, 825.0632f),
                new(146.3309000f, 752.4515f, 756.0107f),
                new(156.9551000f, 751.1077f, 704.1921f),
                new(157.0789000f, 754.6462f, 841.5557f),
                new(184.4552000f, 747.6003f, 617.2846f),
                new(248.7603000f, 723.1207f, 118.5381f),
                new(280.3536700f, 746.5175f, 754.38336f),
                new(293.8393000f, 739.3859f, 531.169f),
                new(310.4404000f, 742.014f, 567.1605f),
                new(367.9469000f, 744.048f, 639.2587f),
                new(371.0945000f, 737.926f, 491.7239f),
                new(378.1241000f, 724.9152f, 287.1851f),
                new(432.9954000f, 731.984f, 568.7686f),
                new(448.9699000f, 725.0576f, 457.0699f),
                new(460.4148000f, 723.1206f, 311.0332f),
                new(469.0294000f, 726.3409f, 535.0562f)
            ]
        }, {
            827, [
                // Hydatos
                new(-933.61840f, 523.23627f, -736.6334f),
                new(-920.34970f, 505.9977f, -172.3633f),
                new(-863.59470f, 513.31824f, -440.51706f),
                new(-812.44620f, 515.9799f, -936.0333f),
                new(-799.73020f, 505.52838f, -109.44379f),
                new(-678.84076f, 504.2658f, -366.4915f),
                new(-665.98303f, 509.72754f, -589.6073f),
                new(-614.09150f, 511.94556f, -872.6113f),
                new(-562.17500f, 496.6946f, -13.687779f),
                new(-438.47156f, 505.1122f, -682.6366f),
                new(-350.35544f, 500.44604f, -108.943886f),
                new(-322.95847f, 500.96313f, -730.7684f),
                new(-182.39871f, 501.35736f, -591.2992f),
                new(-159.06465f, 495.79843f, -44.438065f),
                new(008.371242f, 504.53586f, -368.8599f),
                new(122.102020f, 499.99933f, -616.178f),
                new(140.437470f, 496.05386f, 12.449963f),
                new(172.528400f, 503.0239f, -349.57162f),
                new(226.805160f, 505.52136f, -886.2001f),
                new(287.751900f, 505.17682f, -519.0092f),
                new(344.529880f, 495.0796f, -212.5871f),
                new(368.694950f, 505.93457f, -400.701f),
                new(467.720280f, 505.08575f, -710.6624f),
                new(580.401500f, 497.0735f, -80.38016f),
                new(659.867600f, 502.9848f, -732.67096f),
                new(671.426450f, 508.26538f, -341.5508f),
                new(832.605500f, 513.5617f, -520.9632f),
                new(843.430500f, 494.44394f, -7.3525977f),
                new(851.303200f, 517.4547f, -883.74927f),
                new(872.144700f, 518.4930f, -237.8645f)
            ]
        }
    };
    }
}