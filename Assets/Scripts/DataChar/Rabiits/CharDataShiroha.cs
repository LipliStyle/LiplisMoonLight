//====================================================================
//  ClassName : CharDataBase
//  概要      : キャラクタデータベースクラス
//              
//
//  LiplisLive2D
//  Copyright(c) 2017-2017 sachin. All Rights Reserved. 
//====================================================================
using Assets.Scripts.Common;
using Assets.Scripts.DataChar.CharacterTalk;
using Assets.Scripts.Define;
using Assets.Scripts.LiplisSystem.Msg;

namespace Assets.Scripts.DataChar.Rabiits
{
    public class CharDataShiroha : CharDataBase
    {
        ///=============================
        /// 設定プロパティ

        //アロケーションID
        public int ALLOCATION_ID = 1;

        //葉月 Y座標
        //public const float CARACTER_LOCATION_Y = -2.2f;
        public const float CARACTER_LOCATION_Y = -97;

        //葉月デフォルト位置
        public MST_CARACTER_POSITION DEFAULT_POSITION = MST_CARACTER_POSITION.Right;

        /// <summary>
        /// キャラクターデータ作成
        /// </summary>
        /// <returns></returns>
        public CharacterData CreateCharData()
        {
            return base.CreateCharData(LIVE2D_CANVAS.L2DC_Shiroha_F, LIVE2D_CANVAS.L2DC_Shiroha_R, LIVE2D_CANVAS.L2DC_Shiroha_L, ALLOCATION_ID, DEFAULT_POSITION, PREFAB_NAMES.WINDOW_TALK_S_2, CARACTER_LOCATION_Y);
        }

        /// <summary>
        /// グリートデータを生成する
        /// </summary>
        /// <returns></returns>
        protected override CharDataGreet CreateCharDataGreet(CharDataTone Tone, int allocationID)
        {
            CharDataGreet Greet = new CharDataGreet(Tone, allocationID);

            //グリーとデータ生成
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "おはウサ、マスター。ねむー。", EMOTION.JOY, -3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "おはウサー・・・。マスター。もも姉まだ寝てるよー。", EMOTION.NORMAL, 0, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "もう朝ー？まだねむいよー、くろ姉・・・。", EMOTION.JOY, -3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "もう朝ー？まだねむいよー、つき姉・・・。", EMOTION.JOY, -3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "うさうさうー！やほー、マスター。", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "うさうさうー！、マスター。", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(17, 0, 0), DateUtil.CreateDatetime(22, 0, 0), "こんばんむーん、マスター。", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(17, 0, 0), DateUtil.CreateDatetime(22, 0, 0), "バンワむーん、マスター。", EMOTION.NORMAL, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(22, 0, 0), DateUtil.CreateDatetime(3, 0, 0), "夜遅くまでご苦労だねぇ。白はもう眠いよぉ・・・。", EMOTION.JOY, -3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(18, 0, 0), DateUtil.CreateDatetime(21, 0, 0), "今日も1日よく働いたー。", EMOTION.NORMAL, 3, allocationID));

            return Greet;
        }

        /// <summary>
        /// トーンデータを生成する
        /// </summary>
        /// <returns></returns>
        protected override CharDataTone CreateCharDataTone()
        {
            CharDataTone Tone = new CharDataTone();

            //口調データ生成
            Tone.AddToneData(@"あなた", @"ご主人様");
            Tone.AddToneData(@"えない", @"えません");
            Tone.AddToneData(@"おっと", @"あっ");
            Tone.AddToneData(@"おり", @"おりまして、");
            Tone.AddToneData(@"こんにちは", @"ごきげんよう");
            Tone.AddToneData(@"さようなら", @"ごきげんよう");
            Tone.AddToneData(@"さん", @"様");
            Tone.AddToneData(@"しかし", @"それにしても");
            Tone.AddToneData(@"しますので", @"いたしますので");
            Tone.AddToneData(@"するとき", @"いたしますとき");
            Tone.AddToneData(@"するには", @"なさいます場合には");
            Tone.AddToneData(@"せない", @"せません");
            Tone.AddToneData(@"そうだろ", @"そうでしょう");
            Tone.AddToneData(@"そんな", @"その様な");
            Tone.AddToneData(@"だから", @"ですから");
            Tone.AddToneData(@"だけど", @"ですが");
            Tone.AddToneData(@"だが", @"ですが");
            Tone.AddToneData(@"ダメ", @"ダメです！");
            Tone.AddToneData(@"だろ", @"でしょう");
            Tone.AddToneData(@"ったし", @"ったですし");
            Tone.AddToneData(@"という", @"といいます");
            Tone.AddToneData(@"とすれば", @"としますと");
            Tone.AddToneData(@"とても", @"とっても");
            Tone.AddToneData(@"どんな", @"どの様な");
            Tone.AddToneData(@"なぜ", @"どうして");
            Tone.AddToneData(@"なった", @"なりました");
            Tone.AddToneData(@"なので", @"ですので");
            Tone.AddToneData(@"もっと", @"もう少々");
            Tone.AddToneData(@"やれば", @"なされば");
            Tone.AddToneData(@"やったら", @"なされば");
            Tone.AddToneData(@"よろしく", @"宜しく");
            Tone.AddToneData(@"いくぶん", @"いくらか");
            Tone.AddToneData(@"こいつ", @"この方");
            Tone.AddToneData(@"(ぬ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ません$2");
            Tone.AddToneData(@"(ね)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ね～♪$2");
            Tone.AddToneData(@"(ネ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ネ～♪$2");
            Tone.AddToneData(@"(へ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"へです $2");
            Tone.AddToneData(@"(か)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"でしょうか$2");
            Tone.AddToneData(@"(き|け|し|じ|の|から|ける|そう|たり|ない|なく|なし|など|なら)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"$1です$2");
            Tone.AddToneData(@"(い|か|て|れ|も|り|る|あり|だけ|のみ|はい|べき|べし|ほど|まい|まで|やら|たから|だから|らしい)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"$1です$2");
            Tone.AddToneData(@"(た|ある|たい|かかる|たった|とある)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"$1です$2");
            Tone.AddToneData(@"(だ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"です$2");
            Tone.AddToneData(@"(うめえ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"うまいです$2");
            Tone.AddToneData(@"(する)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"いたします。$2");
            Tone.AddToneData(@"(いや|いな)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"いですね$2");
            Tone.AddToneData(@"(いと)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"いといけません$2");
            Tone.AddToneData(@"(いか)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"いのでしょうか$2");
            Tone.AddToneData(@"(うわ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"います$2");
            Tone.AddToneData(@"(かな|かも)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"かもしれません$2");
            Tone.AddToneData(@"(より)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"からです$2");
            Tone.AddToneData(@"(こう)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"きましょう$2");
            Tone.AddToneData(@"(けど)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"けど・・・$2");
            Tone.AddToneData(@"(じゃ|やん|では)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ではないのでしょうか$2");
            Tone.AddToneData(@"(たし)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"たのですが・・・$2");
            Tone.AddToneData(@"(たら)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"たら～♪$2");
            Tone.AddToneData(@"(れよ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ってください$2");
            Tone.AddToneData(@"(って)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ってな具合です$2");
            Tone.AddToneData(@"(っと)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"っとあうあうです・・・$2");
            Tone.AddToneData(@"(とく)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ておきましょう$2");
            Tone.AddToneData(@"(たろ|だろ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"でしょう？$2");
            Tone.AddToneData(@"(だが)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ですが・・・$2");
            Tone.AddToneData(@"(だね)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ですね$2");
            Tone.AddToneData(@"(なぁ|なあ|なー)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ですねぇ・・・$2");
            Tone.AddToneData(@"(だぜ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ですよ！。$2");
            Tone.AddToneData(@"(だぞ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ですよ？$2");
            Tone.AddToneData(@"(たる)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"てやりますです～！$2");
            Tone.AddToneData(@"(てよ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"て頂けないでしょうか$2");
            Tone.AddToneData(@"(とか)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"とかです$2");
            Tone.AddToneData(@"(とも)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"とも言います$2");
            Tone.AddToneData(@"(なれ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"なってください$2");
            Tone.AddToneData(@"(なる|なり)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"なります$2");
            Tone.AddToneData(@"(にて)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"にて$2");
            Tone.AddToneData(@"(ねー)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ね～♪$2");
            Tone.AddToneData(@"(ねぇ|ねえ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ねぇ・・・$2");
            Tone.AddToneData(@"(ねん)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ねん$2");
            Tone.AddToneData(@"(ので)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"のです$2");
            Tone.AddToneData(@"(のに)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"のに・・・$2");
            Tone.AddToneData(@"(もん)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"もん！$2");
            Tone.AddToneData(@"(やめろ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"やめてください！$2");
            Tone.AddToneData(@"(すこ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"好きです$2");
            Tone.AddToneData(@"(ちなむ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ちなむそうです$2");
            Tone.AddToneData(@"(ありがと|ありがとう)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ありがとうございます$2");
            Tone.AddToneData(@"(いたします|します)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"いたします$2");
            Tone.AddToneData(@"(うーん)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"うーん・・・$2");
            Tone.AddToneData(@"(おめでとう)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"おめでとうございます！$2");
            Tone.AddToneData(@"(お疲れさま)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"お疲れさまです$2");
            Tone.AddToneData(@"(がっかり)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"がっかりですね・・・$2");
            Tone.AddToneData(@"(ぐらい)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ぐらいです$2");
            Tone.AddToneData(@"(けれど)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"けれど・・・$2");
            Tone.AddToneData(@"(しばしば)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"しばしばです$2");
            Tone.AddToneData(@"(じゃん)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"じゃないでしょうか$2");
            Tone.AddToneData(@"(そこら)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"そこら辺です$2");
            Tone.AddToneData(@"(そのため)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"そのためです$2");
            Tone.AddToneData(@"(それだけ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"それだけです$2");
            Tone.AddToneData(@"(それ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"それです$2");
            Tone.AddToneData(@"(いっぱい)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"たくさん$2");
            Tone.AddToneData(@"(だけど)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"だけど・・・$2");
            Tone.AddToneData(@"(だって)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"だそうです$2");
            Tone.AddToneData(@"(ってな)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ってな感じです$2");
            Tone.AddToneData(@"(ていた)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ておりました$2");
            Tone.AddToneData(@"(ている)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ております$2");
            Tone.AddToneData(@"(でしょ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"でしょう$2");
            Tone.AddToneData(@"(ござる)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"です$2");
            Tone.AddToneData(@"(なのに)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ですのに$2");
            Tone.AddToneData(@"(ではない)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ではありません$2");
            Tone.AddToneData(@"(てない)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"てません$2");
            Tone.AddToneData(@"(ていう)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"て言う感じです$2");
            Tone.AddToneData(@"(という)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"というそうです$2");
            Tone.AddToneData(@"(といふ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"というそうです$2");
            Tone.AddToneData(@"(といった)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"といったそうです$2");
            Tone.AddToneData(@"(どうか)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"どうか、どうでしょうねぇ$2");
            Tone.AddToneData(@"(どうせ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"どうせいや、ダメじゃないのです！$2");
            Tone.AddToneData(@"(どうぞ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"どうぞ♪$2");
            Tone.AddToneData(@"(ないし)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ないですし $2");
            Tone.AddToneData(@"(なきゃ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"なきゃです！$2");
            Tone.AddToneData(@"(だお)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"です$2");
            Tone.AddToneData(@"(だった)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"でした$2");
            Tone.AddToneData(@"(げよ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"げて下さい$2");
            Tone.AddToneData(@"(ならない)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"なりません$2");
            Tone.AddToneData(@"(なるほど)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"なるほどです！$2");
            Tone.AddToneData(@"(にあたる)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"にあたります$2");
            Tone.AddToneData(@"(について)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"についてです$2");
            Tone.AddToneData(@"(にとって)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"にとって・・・$2");
            Tone.AddToneData(@"(によって)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"によりまして$2");
            Tone.AddToneData(@"(による)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"によります$2");
            Tone.AddToneData(@"(にわたる)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"にわたります$2");
            Tone.AddToneData(@"(に当たります)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"に当たります$2");
            Tone.AddToneData(@"(ばかり|ばっか|ばっかり)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ばかりです$2");
            Tone.AddToneData(@"(めない)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"めません$2");
            Tone.AddToneData(@"(ものの)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"ものの・・・$2");
            Tone.AddToneData(@"(らない|りない)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"りません$2");
            Tone.AddToneData(@"(れない)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"れません$2");
            Tone.AddToneData(@"(んじゃ)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"んじゃないでしょうか$2");
            Tone.AddToneData(@"(すまん)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"申し訳ありません$2");
            Tone.AddToneData(@"(いただきます)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"頂戴いたします$2");
            Tone.AddToneData(@"(ほんとうに)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"本当に、本当に・・・$2");
            Tone.AddToneData(@"(w|ｗ|W|Ｗ+)(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"です$1$2");
            Tone.AddToneData(@"([\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}\p{IsCJKUnifiedIdeographsExtensionA}]|[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF])(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"$1です$2");
            Tone.AddToneData(@"([ア-ヴ])(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"$1です$2");
            Tone.AddToneData(@"([0-9])(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"$1です$2");
            Tone.AddToneData(@"([０-９])(。|｡|．|！|!|？|\?|\)|」|』|…|―|$)", @"$1です$2");

            return Tone;
        }
    }
}
