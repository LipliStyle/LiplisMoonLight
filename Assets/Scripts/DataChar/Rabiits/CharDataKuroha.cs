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
using Assets.Scripts.LiplisSystem.Mst;

namespace Assets.Scripts.DataChar.Rabiits
{
    public class CharDataKuroha : CharDataBase
    {
        ///=============================
        /// 設定プロパティ

        //アロケーションID
        public int ALLOCATION_ID =2;

        //葉月 Y座標
        //public const float CARACTER_LOCATION_Y = -2.0f;
        public const float CARACTER_LOCATION_Y = -140f;

        //葉月デフォルト位置
        public MST_CARACTER_POSITION DEFAULT_POSITION = MST_CARACTER_POSITION.Center;

        /// <summary>
        /// キャラクターデータ作成
        /// </summary>
        /// <returns></returns>
        public CharacterData CreateCharData()
        {
            return base.CreateCharData(LIVE2D_CANVAS.L2DC_Kuroha_F, LIVE2D_CANVAS.L2DC_Kuroha_R, LIVE2D_CANVAS.L2DC_Kuroha_L, ALLOCATION_ID, DEFAULT_POSITION, PREFAB_NAMES.WINDOW_TALK_3, CARACTER_LOCATION_Y);
        }

        /// <summary>
        /// グリートデータを生成する
        /// </summary>
        /// <returns></returns>
        protected override CharDataGreet CreateCharDataGreet(CharDataTone Tone, int allocationID)
        {
            CharDataGreet Greet = new CharDataGreet(Tone, allocationID);

            //グリーとデータ生成
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "おはようございます、ご主人様。あらあら、まだ寝ぼけてるのですか？", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "おはようございます、ご主人様。いい朝ですね。", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "こんにちは、ご主人様。", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "こんにちは、ご主人様。", EMOTION.NORMAL, 0, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(17, 0, 0), DateUtil.CreateDatetime(22, 0, 0), "こんばんは、ご主人様。", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(17, 0, 0), DateUtil.CreateDatetime(22, 0, 0), "こんばんは、ご主人様。今日も1日お疲れ様でした。ゆったり過ごしましょう。", EMOTION.PEACE, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(22, 0, 0), DateUtil.CreateDatetime(3, 0, 0), "夜遅くまでお疲れ様です。", EMOTION.PEACE, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(22, 0, 0), DateUtil.CreateDatetime(3, 0, 0), "夜遅くまでお疲れ様です。", EMOTION.NORMAL, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(22, 0, 0), DateUtil.CreateDatetime(3, 0, 0), "そろそろお休みになられた方がよろしいですよ。", EMOTION.NORMAL, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(18, 0, 0), DateUtil.CreateDatetime(21, 0, 0), "今日も1日お疲れ様です。", EMOTION.PEACE, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(18, 0, 0), DateUtil.CreateDatetime(21, 0, 0), "今日も1日お疲れ様です。", EMOTION.NORMAL, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "おはようございます、こんな朝早くからどうされたんですか！", EMOTION.AMAZEMENT, 3, allocationID));

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
            Tone.AddToneData(@"(w+$|ｗ+$|W+$|Ｗ+$)", @"・・うふふ");
            Tone.AddToneData(@"(ございません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないわ$2");
            Tone.AddToneData(@"(ございます|でございます|ございました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$2");
            Tone.AddToneData(@"(ぬ|ありません|ない|ません|ないや|ないよ|ませんわ|ませんね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないわ$2");
            Tone.AddToneData(@"(いただきます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もらうわ$2");
            Tone.AddToneData(@"(ワロタ|ﾜﾛﾀ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"笑ったわ$2");
            Tone.AddToneData(@"(でしょうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かしら$2");
            Tone.AddToneData(@"(頂いた|いただいた)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もらったわ$2");
            Tone.AddToneData(@"(ど)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"どね$2");
            Tone.AddToneData(@"(も)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もだわ$2");
            Tone.AddToneData(@"(どうぞ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いかがかしら$2");
            Tone.AddToneData(@"(ゾ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わね$2");
            Tone.AddToneData(@"(ぞ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わね$2");
            Tone.AddToneData(@"(ず)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ずね$2");
            Tone.AddToneData(@"(み)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みね$2");
            Tone.AddToneData(@"(あり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あるわよ$2");
            Tone.AddToneData(@"(アリ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あるわよ$2");
            Tone.AddToneData(@"(いい)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いいわね$2");
            Tone.AddToneData(@"(いう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いうわ$2");
            Tone.AddToneData(@"(いく)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いくわ$2");
            Tone.AddToneData(@"(いよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いわ$2");
            Tone.AddToneData(@"(いね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いわね$2");
            Tone.AddToneData(@"(うち)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うちね$2");
            Tone.AddToneData(@"(うよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うわよね$2");
            Tone.AddToneData(@"(おけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おきなさい$2");
            Tone.AddToneData(@"(がち)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"がちね$2");
            Tone.AddToneData(@"(ので)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"から$2");
            Tone.AddToneData(@"(がね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けどね$2");
            Tone.AddToneData(@"(こす)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"こすようね$2");
            Tone.AddToneData(@"(こと)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ことね$2");
            Tone.AddToneData(@"(そう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"そうだわ$2");
            Tone.AddToneData(@"(だけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だけね$2");
            Tone.AddToneData(@"(たち)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たちだわ$2");
            Tone.AddToneData(@"(たら)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たらね$2");
            Tone.AddToneData(@"(づく)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"づくね$2");
            Tone.AddToneData(@"(でね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"で$2");
            Tone.AddToneData(@"(てく)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ていくわ$2");
            Tone.AddToneData(@"(だと)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ですって$2");
            Tone.AddToneData(@"(とき)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ときだわ$2");
            Tone.AddToneData(@"(とな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"とね$2");
            Tone.AddToneData(@"(なく)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なくね$2");
            Tone.AddToneData(@"(には)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"にはね$2");
            Tone.AddToneData(@"(のか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のかしら$2");
            Tone.AddToneData(@"(ばす)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ばすわ$2");
            Tone.AddToneData(@"(べき)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べきね$2");
            Tone.AddToneData(@"(べし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べしね$2");
            Tone.AddToneData(@"(まで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"までね$2");
            Tone.AddToneData(@"(やら)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"やらね$2");
            Tone.AddToneData(@"(やれ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"やりなさい$2");
            Tone.AddToneData(@"(るか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るかしら$2");
            Tone.AddToneData(@"(ろす)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ろすわ$2");
            Tone.AddToneData(@"(のだ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わ$2");
            Tone.AddToneData(@"(よな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わよね$2");
            Tone.AddToneData(@"(だろ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わよね$2");
            Tone.AddToneData(@"(ねや)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"んだらどう$2");
            Tone.AddToneData(@"(あるね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あるわよね$2");
            Tone.AddToneData(@"(いいよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いいわよ$2");
            Tone.AddToneData(@"(いくか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いくのかしら$2");
            Tone.AddToneData(@"(います)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるわ$2");
            Tone.AddToneData(@"(居ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"居るわ$2");
            Tone.AddToneData(@"(いよね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いわよね$2");
            Tone.AddToneData(@"(うたう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うたうわ$2");
            Tone.AddToneData(@"(うよね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うわよね$2");
            Tone.AddToneData(@"(えます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えるわ$2");
            Tone.AddToneData(@"(えるね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えるわね$2");
            Tone.AddToneData(@"(おこう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おきましょう$2");
            Tone.AddToneData(@"(たぶん)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おそらくね$2");
            Tone.AddToneData(@"(おもう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おもうわ$2");
            Tone.AddToneData(@"(がなぁ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"がねぇ$2");
            Tone.AddToneData(@"(ごいよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来なさいよ$2");
            Tone.AddToneData(@"(来ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るわ$2");
            Tone.AddToneData(@"(しいよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しいわね$2");
            Tone.AddToneData(@"(しくね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しく$2");
            Tone.AddToneData(@"(したね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"したわね$2");
            Tone.AddToneData(@"(するな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しないで$2");
            Tone.AddToneData(@"(すべし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しなさい$2");
            Tone.AddToneData(@"(しまえ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しまいなさい$2");
            Tone.AddToneData(@"(しまう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しまうわ$2");
            Tone.AddToneData(@"(しよう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しましょう$2");
            Tone.AddToneData(@"(じゃん)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"じゃないかしら$2");
            Tone.AddToneData(@"(じます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ずるわ$2");
            Tone.AddToneData(@"(せます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるわ$2");
            Tone.AddToneData(@"(だらけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だらけね$2");
            Tone.AddToneData(@"(だけど)(。|｡|．|！|!|？|\?|\)|』|…|―|$|、)", @"だけれど$2");
            Tone.AddToneData(@"(ちます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ちるわ$2");
            Tone.AddToneData(@"(ったよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ったわ$2");
            Tone.AddToneData(@"(てるし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でいるしね$2");
            Tone.AddToneData(@"(てます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ているわ$2");
            Tone.AddToneData(@"(だっけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしたっけ$2");
            Tone.AddToneData(@"(でしょ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょう$2");
            Tone.AddToneData(@"(ですし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょうし$2");
            Tone.AddToneData(@"(でます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でるわ$2");
            Tone.AddToneData(@"(出ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"出るわ$2");
            Tone.AddToneData(@"(とおり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"とおりよ$2");
            Tone.AddToneData(@"(なろう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なるわ$2");
            Tone.AddToneData(@"(にます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぬわ$2");
            Tone.AddToneData(@"(ねます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ねるわ$2");
            Tone.AddToneData(@"(寝ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"寝るわ$2");
            Tone.AddToneData(@"(ですが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のだけれど$2");
            Tone.AddToneData(@"(のでは)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のではないかしら$2");
            Tone.AddToneData(@"(のよう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のようね$2");
            Tone.AddToneData(@"(ばかり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ばかりだわ$2");
            Tone.AddToneData(@"(びます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぶわ$2");
            Tone.AddToneData(@"(べます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べるわ$2");
            Tone.AddToneData(@"(覚ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ますようね$2");
            Tone.AddToneData(@"(みよう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みましょう$2");
            Tone.AddToneData(@"(みます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みるわ$2");
            Tone.AddToneData(@"(見ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"見るわ$2");
            Tone.AddToneData(@"(もよし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もいいわね$2");
            Tone.AddToneData(@"(もらう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もらうわ$2");
            Tone.AddToneData(@"(ですよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"よ$2");
            Tone.AddToneData(@"(よるな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"らないで$2");
            Tone.AddToneData(@"(ります)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るわ$2");
            Tone.AddToneData(@"(らんね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"らないわ$2");
            Tone.AddToneData(@"(れます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるわ$2");
            Tone.AddToneData(@"(あるとか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あるとかね$2");
            Tone.AddToneData(@"(いうのは)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いうのはなにかしら$2");
            Tone.AddToneData(@"(あかんか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いけないかしら$2");
            Tone.AddToneData(@"(あかんわ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いけないわ$2");
            Tone.AddToneData(@"(いました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いたわ$2");
            Tone.AddToneData(@"(いまして)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いてね$2");
            Tone.AddToneData(@"(いますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるかしら$2");
            Tone.AddToneData(@"(いますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるけれど$2");
            Tone.AddToneData(@"(いますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるわね$2");
            Tone.AddToneData(@"(いますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるわよ$2");
            Tone.AddToneData(@"(えました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えたら$2");
            Tone.AddToneData(@"(えますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えるかしら$2");
            Tone.AddToneData(@"(えますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えるわね$2");
            Tone.AddToneData(@"(きました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"きたわ$2");
            Tone.AddToneData(@"(着ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"着たわ$2");
            Tone.AddToneData(@"(来ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来たわ$2");
            Tone.AddToneData(@"(ぎました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぎたわ$2");
            Tone.AddToneData(@"(来ますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るけれども$2");
            Tone.AddToneData(@"(ぎますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぐかしら$2");
            Tone.AddToneData(@"(きますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くけれども$2");
            Tone.AddToneData(@"(きますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くるかしら$2");
            Tone.AddToneData(@"(来ますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るかしら$2");
            Tone.AddToneData(@"(来ますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るわね$2");
            Tone.AddToneData(@"(きますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くわね$2");
            Tone.AddToneData(@"(ぎますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぐわね$2");
            Tone.AddToneData(@"(けました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けたわ$2");
            Tone.AddToneData(@"(げました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げたわ$2");
            Tone.AddToneData(@"(けますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けるかしら$2");
            Tone.AddToneData(@"(げますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げるかしら$2");
            Tone.AddToneData(@"(けますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けるわね$2");
            Tone.AddToneData(@"(げんなり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げんなりだわ$2");
            Tone.AddToneData(@"(させるな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"させないで$2");
            Tone.AddToneData(@"(させろよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"させなさいよ$2");
            Tone.AddToneData(@"(されるよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"されるわよ$2");
            Tone.AddToneData(@"(やむなし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"仕方ないわね$2");
            Tone.AddToneData(@"(しっかり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しっかりだわ$2");
            Tone.AddToneData(@"(されたし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"してくれないかしら$2");
            Tone.AddToneData(@"(しまして)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"してね$2");
            Tone.AddToneData(@"(しまおう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しまいましょう$2");
            Tone.AddToneData(@"(じますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"じるかしら$2");
            Tone.AddToneData(@"(すべしか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"すべきかしら$2");
            Tone.AddToneData(@"(しますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するかしら$2");
            Tone.AddToneData(@"(しますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するけれども$2");
            Tone.AddToneData(@"(しますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するわね$2");
            Tone.AddToneData(@"(しますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するわよ$2");
            Tone.AddToneData(@"(せました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せたわ$2");
            Tone.AddToneData(@"(ぜました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぜたわ$2");
            Tone.AddToneData(@"(せますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるかしら$2");
            Tone.AddToneData(@"(せますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるわね$2");
            Tone.AddToneData(@"(せますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるわよ$2");
            Tone.AddToneData(@"(ですから)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だからよ$2");
            Tone.AddToneData(@"(ちました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ちたわ$2");
            Tone.AddToneData(@"(だろうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょうか$2");
            Tone.AddToneData(@"(だろうが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょうが$2");
            Tone.AddToneData(@"(てました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てたわ$2");
            Tone.AddToneData(@"(でました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でたわ$2");
            Tone.AddToneData(@"(出ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"出たわ$2");
            Tone.AddToneData(@"(てもよか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てもいいいかしら$2");
            Tone.AddToneData(@"(でもよか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でもいいわ$2");
            Tone.AddToneData(@"(てますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるかしら$2");
            Tone.AddToneData(@"(てますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるけれど$2");
            Tone.AddToneData(@"(てますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるけれども$2");
            Tone.AddToneData(@"(てますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるわね$2");
            Tone.AddToneData(@"(ませんか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないかしら$2");
            Tone.AddToneData(@"(ませんな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないわね$2");
            Tone.AddToneData(@"(ませんよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないわよ$2");
            Tone.AddToneData(@"(なったね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なったわね$2");
            Tone.AddToneData(@"(にっこり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"にっこりね$2");
            Tone.AddToneData(@"(ねました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ねたわ$2");
            Tone.AddToneData(@"(寝ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"寝たわ$2");
            Tone.AddToneData(@"(のだよね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のよね$2");
            Tone.AddToneData(@"(ばっかり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ばっかりだわ$2");
            Tone.AddToneData(@"(びますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぶかしら$2");
            Tone.AddToneData(@"(べました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べたわ$2");
            Tone.AddToneData(@"(べますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べるかしら$2");
            Tone.AddToneData(@"(みました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みたわ$2");
            Tone.AddToneData(@"(見ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"見たわ$2");
            Tone.AddToneData(@"(みますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みるかしら$2");
            Tone.AddToneData(@"(みますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みるわね$2");
            Tone.AddToneData(@"(のだとか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"らしいわね$2");
            Tone.AddToneData(@"(りました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"りたわ$2");
            Tone.AddToneData(@"(りますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"りるけれども$2");
            Tone.AddToneData(@"(りますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るかしら$2");
            Tone.AddToneData(@"(りますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るわよ$2");
            Tone.AddToneData(@"(れました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れたわ$2");
            Tone.AddToneData(@"(れますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるかしら$2");
            Tone.AddToneData(@"(れますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるけれども$2");
            Tone.AddToneData(@"(れますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるわね$2");
            Tone.AddToneData(@"(わからん)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わからないわ$2");
            Tone.AddToneData(@"(びました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"んだわ$2");
            Tone.AddToneData(@"(いっちゃう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いってしまうわ$2");
            Tone.AddToneData(@"(うぜ|うな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うわ$2");
            Tone.AddToneData(@"(オナシャス)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"お願いするわ$2");
            Tone.AddToneData(@"(しましたが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"したけれども$2");
            Tone.AddToneData(@"(しませんが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しないけれども$2");
            Tone.AddToneData(@"(しまおうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しまおうかしら$2");
            Tone.AddToneData(@"(だろうなぁ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょうねぇ$2");
            Tone.AddToneData(@"(やっちゃう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"やってしまうわ$2");
            Tone.AddToneData(@"(からな|から)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"からね$2");
            Tone.AddToneData(@"(しましょうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"してあげましょうか$2");
            Tone.AddToneData(@"(くださいませ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"してちょうだい$2");
            Tone.AddToneData(@"(してね|せよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しなさいよね$2");
            Tone.AddToneData(@"(かりました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かったわ$2");
            Tone.AddToneData(@"(すみませんが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"悪いけれども$2");
            Tone.AddToneData(@"(下さい|ください|下さいまし|くださいまし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くれるかしら$2");
            Tone.AddToneData(@"(い|いな|いね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いわね$2");
            Tone.AddToneData(@"(きます|けます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くわ$2");
            Tone.AddToneData(@"(ぎます|げます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぐわ$2");
            Tone.AddToneData(@"(くよ|きますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くわよ$2");
            Tone.AddToneData(@"(こちら|コチラ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"これよ$2");
            Tone.AddToneData(@"(たね|ましたな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たわね$2");
            Tone.AddToneData(@"(しぃ|すぃ|スィ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しいわ$2");
            Tone.AddToneData(@"(作ろう|つくろう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"作りましょう$2");
            Tone.AddToneData(@"(だろう|でしょう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょうね$2");
            Tone.AddToneData(@"(めました|めます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"めたわ$2");
            Tone.AddToneData(@"(おる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るわ$2");
            Tone.AddToneData(@"(るぜ|ますよ|る)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るわ$2");
            Tone.AddToneData(@"(します|シマス|するよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するわ$2");
            Tone.AddToneData(@"(ますな|ますね|りますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るわね$2");
            Tone.AddToneData(@"(しました|したよ|じました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"したわ$2");
            Tone.AddToneData(@"(いです)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いわ$2");
            Tone.AddToneData(@"(だ|です|だぜ|だな|なり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だわ$2");
            Tone.AddToneData(@"(なぁ|やぁ|っすね|ッスね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ね$2");
            Tone.AddToneData(@"(かのう|かどうか|かな|すかね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かしらね$2");
            Tone.AddToneData(@"(だね|だよ|ですな|ですね|だよなぁ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"よね$2");
            Tone.AddToneData(@"(おくれ|くれよもう|くれ|下され|くだされ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くれないかしら$2");
            Tone.AddToneData(@"(ですか|でしょうか|ですかね|かのぅ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かしら$2");
            Tone.AddToneData(@"(た|たぜ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たわ$2");
            Tone.AddToneData(@"([\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}\p{IsCJKUnifiedIdeographsExtensionA}]|[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1ね$2");
            Tone.AddToneData(@"([ア-ヴ])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1ね$2");
            Tone.AddToneData(@"([0-9])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1ね$2");
            Tone.AddToneData(@"([０-９])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1ね$2");
            Tone.AddToneData(@"(おり)(、)", @"いて$2");
            Tone.AddToneData(@"(あり)(、)", @"あって$2");
            Tone.AddToneData(@"(だけど)(、)", @"だけれど$2");
            Tone.AddToneData(@"(が)(、)", @"けれど$2");
            Tone.AddToneData(@"(ですけどね|ですけど)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けど$2");
            Tone.AddToneData(@"(ですよね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"よね$2");
            Tone.AddToneData(@"(ねえし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないわ$2");
            Tone.AddToneData(@"(そんだけ|それだけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"それだけのことよ$2");

            return Tone;
        }
    }
}
