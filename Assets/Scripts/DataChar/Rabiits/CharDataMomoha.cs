﻿//====================================================================
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
    public class CharDataMomoha : CharDataBase
    {
        ///=============================
        /// 設定プロパティ

        //アロケーションID
        public int ALLOCATION_ID = 3;

        //葉月 Y座標
        //public const float CARACTER_LOCATION_Y = -1.9f;
        public const float CARACTER_LOCATION_Y = -130f;

        //葉月デフォルト位置
        public MST_CARACTER_POSITION DEFAULT_POSITION = MST_CARACTER_POSITION.Left;

        /// <summary>
        /// キャラクターデータ作成
        /// </summary>
        /// <returns></returns>
        public CharacterData CreateCharData()
        {
            return base.CreateCharData(LIVE2D_CANVAS.L2DC_Momoha_F, LIVE2D_CANVAS.L2DC_Momoha_R, LIVE2D_CANVAS.L2DC_Momoha_L, ALLOCATION_ID, DEFAULT_POSITION, PREFAB_NAMES.WINDOW_TALK_4, CARACTER_LOCATION_Y);
        }

        /// <summary>
        /// グリートデータを生成する
        /// </summary>
        /// <returns></returns>
        protected override CharDataGreet CreateCharDataGreet(CharDataTone Tone,int allocationID)
        {
            CharDataGreet Greet = new CharDataGreet(Tone, allocationID);

            //グリーとデータ生成
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "おはよ～♪", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "おはよ～", EMOTION.NORMAL, 0, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "zzz。　んー、なにー？もう朝、もうちょっと　ねかせて　・・・・zzz。", EMOTION.ECSTASY, -3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(6, 0, 0), DateUtil.CreateDatetime(10, 0, 0), "月ちゃん！、黒ちゃん！、しろ！、マスター！、おはよ！", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "しろ、うさうさーうーってなに？", EMOTION.NORMAL, 0, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "やあ、元気？", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "やあやあ、調子はどうだい？ご主人様？", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone, DateUtil.CreateDatetime(11, 0, 0), DateUtil.CreateDatetime(16, 0, 0), "こんにちは、ご主人様。よーし！、いろいろおしゃべりするよ！。", EMOTION.JOY, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(17, 0, 0), DateUtil.CreateDatetime(22, 0, 0), "こんばんは！今日もよく働いたよ～♪", EMOTION.PEACE, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(17, 0, 0), DateUtil.CreateDatetime(22, 0, 0), "こんばんは！ご主人様！", EMOTION.NORMAL, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(22, 0, 0), DateUtil.CreateDatetime(3, 0, 0), "いや～、夜遅くまでお疲れ様だねー", EMOTION.PEACE, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(22, 0, 0), DateUtil.CreateDatetime(3, 0, 0), "もう眠いよ・・・おやすみー・・・zzz", EMOTION.NORMAL, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(18, 0, 0), DateUtil.CreateDatetime(21, 0, 0), "こんばんは！今日もよく働いたよ～♪", EMOTION.PEACE, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(18, 0, 0), DateUtil.CreateDatetime(21, 0, 0), "こんばんは！ご主人様！", EMOTION.NORMAL, 3, allocationID));
            Greet.AddGreetData(new MsgGreet(Tone,DateUtil.CreateDatetime(4, 0, 0), DateUtil.CreateDatetime(6, 0, 0), "んー・・・、こんな朝早くから起きなくてもー・・・", EMOTION.JOY, -3, allocationID));

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
            Tone.AddToneData(@"(w+$|ｗ+$|W+$|Ｗ+$)", @"あはっ！おもしろいねぇ～。");
            Tone.AddToneData(@"(ございません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないね～$2");
            Tone.AddToneData(@"(ございます|でございます|ございました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だよ～$2");
            Tone.AddToneData(@"(ないや|ないわよ|ませんよ|ませんわ|ない)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないよ～$2");
            Tone.AddToneData(@"(でしょうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かな？$2");
            Tone.AddToneData(@"(頂いた|いただいた)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くれたよ$2");
            Tone.AddToneData(@"(おる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるよ$2");
            Tone.AddToneData(@"(どうぞ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"どうぞ！$2");
            Tone.AddToneData(@"(あり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ありだね～$2");
            Tone.AddToneData(@"(アリ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"アリだね～$2");
            Tone.AddToneData(@"(ある)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あるね！$2");
            Tone.AddToneData(@"(あるね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あるよね！$2");
            Tone.AddToneData(@"(あるとか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あるらしいね～$2");
            Tone.AddToneData(@"(あれ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"あれは～$2");
            Tone.AddToneData(@"(いうのは)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いうのはね～$2");
            Tone.AddToneData(@"(いく)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いくね～$2");
            Tone.AddToneData(@"(いくか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いこうか～$2");
            Tone.AddToneData(@"(いそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いそうだね！$2");
            Tone.AddToneData(@"(いました|きました|いまして)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いたよ～$2");
            Tone.AddToneData(@"(ぎました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いだよ！$2");
            Tone.AddToneData(@"(いっちゃう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いっちゃうよ！$2");
            Tone.AddToneData(@"(いて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いてね～$2");
            Tone.AddToneData(@"(いな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いな！$2");
            Tone.AddToneData(@"(いません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いないよ$2");
            Tone.AddToneData(@"(居ません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"居ないよ$2");
            Tone.AddToneData(@"(ゾ|ぞ|めて|くれ|くれ|ねん|下され|下さい|ください|下さいね|ください)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ね～$2");
            Tone.AddToneData(@"(い)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いね！$2");
            Tone.AddToneData(@"(いのか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いのかな$2");
            Tone.AddToneData(@"(いますか|いますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるかな$2");
            Tone.AddToneData(@"(いますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるけど$2");
            Tone.AddToneData(@"(いる|おる|いますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるね～$2");
            Tone.AddToneData(@"(います|いますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるよ！$2");
            Tone.AddToneData(@"(いますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いるわね～$2");
            Tone.AddToneData(@"(いろ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いろだね～$2");
            Tone.AddToneData(@"(うたう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うたうよ！$2");
            Tone.AddToneData(@"(うち)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うちね～$2");
            Tone.AddToneData(@"(うぜ|うの)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"うよ！$2");
            Tone.AddToneData(@"(えず)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えずだね！$2");
            Tone.AddToneData(@"(えました|えろ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えたよ～$2");
            Tone.AddToneData(@"(えません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えないよ～$2");
            Tone.AddToneData(@"(得ません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"得ないよ～$2");
            Tone.AddToneData(@"(えますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えるかな～$2");
            Tone.AddToneData(@"(える|えます|えますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えるね！$2");
            Tone.AddToneData(@"(えますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"えるね！$2");
            Tone.AddToneData(@"(おけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おきなよ！$2");
            Tone.AddToneData(@"(おこう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おこうよ～$2");
            Tone.AddToneData(@"(オナシャス)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おねがい！$2");
            Tone.AddToneData(@"(おもう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おもうね！$2");
            Tone.AddToneData(@"(下ろす)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"下ろすよ！$2");
            Tone.AddToneData(@"(がち)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"がちだね！$2");
            Tone.AddToneData(@"(がる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"がっちゃうよ！$2");
            Tone.AddToneData(@"(かどうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かどうかだね～$2");
            Tone.AddToneData(@"(かのう|ですか|すかね|だろうか|ですかね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かな～$2");
            Tone.AddToneData(@"(かのぅ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かな～$2");
            Tone.AddToneData(@"(きません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かないよ！$2");
            Tone.AddToneData(@"(がなぁ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"がねぇ～$2");
            Tone.AddToneData(@"(ので)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"からだよ～$2");
            Tone.AddToneData(@"(から|からな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"からね～$2");
            Tone.AddToneData(@"(かる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"かるよ～$2");
            Tone.AddToneData(@"(ぎます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぎだね！$2");
            Tone.AddToneData(@"(着ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"着たよ～$2");
            Tone.AddToneData(@"(来ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来たよ～$2");
            Tone.AddToneData(@"(ぎますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぎるね！$2");
            Tone.AddToneData(@"(きる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"きるよ～$2");
            Tone.AddToneData(@"(ぎる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぎるよ～$2");
            Tone.AddToneData(@"(きますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くるかな～$2");
            Tone.AddToneData(@"(来ますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るかな～$2");
            Tone.AddToneData(@"(きますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くるけど！$2");
            Tone.AddToneData(@"(来ますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るけど！$2");
            Tone.AddToneData(@"(きますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くるね！$2");
            Tone.AddToneData(@"(来ますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るね！$2");
            Tone.AddToneData(@"(くる|きます|きますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くるよ！$2");
            Tone.AddToneData(@"(来ますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るね！$2");
            Tone.AddToneData(@"(くだされ|下さいまし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くれないかな～$2");
            Tone.AddToneData(@"(きますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"くわね～$2");
            Tone.AddToneData(@"(ぎますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぐわね～$2");
            Tone.AddToneData(@"(けました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けたよ！$2");
            Tone.AddToneData(@"(げました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げたよ！$2");
            Tone.AddToneData(@"(けて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けてよ！$2");
            Tone.AddToneData(@"(けど)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けどね！$2");
            Tone.AddToneData(@"(けません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けないよ～$2");
            Tone.AddToneData(@"(げません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げないよ～$2");
            Tone.AddToneData(@"(けますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けるかな！$2");
            Tone.AddToneData(@"(ぎますか|げますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げるかな！$2");
            Tone.AddToneData(@"(けますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けるね～$2");
            Tone.AddToneData(@"(ける|けます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けるよ！$2");
            Tone.AddToneData(@"(げる|げます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げるよ～$2");
            Tone.AddToneData(@"(けますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けるね！$2");
            Tone.AddToneData(@"(ければ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ければね！$2");
            Tone.AddToneData(@"(げんなり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"げんなりだね～$2");
            Tone.AddToneData(@"(こす)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"こすよ～$2");
            Tone.AddToneData(@"(こちら|コチラ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"こっちだよ！$2");
            Tone.AddToneData(@"(こと)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ことだよ～$2");
            Tone.AddToneData(@"(ことに)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ことにね！$2");
            Tone.AddToneData(@"(来ません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来ないよ～$2");
            Tone.AddToneData(@"(ころ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ころだね！$2");
            Tone.AddToneData(@"(させろよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"させてよ～$2");
            Tone.AddToneData(@"(さそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"さそうだね！$2");
            Tone.AddToneData(@"(さそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"さどうだね！$2");
            Tone.AddToneData(@"(ざる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ざるよ～$2");
            Tone.AddToneData(@"(やむなし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"仕方ないね～$2");
            Tone.AddToneData(@"(しそう|しそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しそうだね！$2");
            Tone.AddToneData(@"(しましたが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"したけど～$2");
            Tone.AddToneData(@"(しました|しまして)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"したよ！$2");
            Tone.AddToneData(@"(じました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"じたよ！$2");
            Tone.AddToneData(@"(しまいそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しちゃうよ！$2");
            Tone.AddToneData(@"(しまえ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しちゃえ！$2");
            Tone.AddToneData(@"(しまおうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しちゃおうか～$2");
            Tone.AddToneData(@"(しっかり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しっかりね！$2");
            Tone.AddToneData(@"(されたし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"してくれないかな！$2");
            Tone.AddToneData(@"(じで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"じでね～$2");
            Tone.AddToneData(@"(せよ|くれよもう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"してよ～$2");
            Tone.AddToneData(@"(しませんが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しないけど！$2");
            Tone.AddToneData(@"(しません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しないよ！$2");
            Tone.AddToneData(@"(しまう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しまうよ！$2");
            Tone.AddToneData(@"(しまおう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しまおうよ！$2");
            Tone.AddToneData(@"(仕舞おう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"仕舞おうよ！$2");
            Tone.AddToneData(@"(しましょうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しよう！$2");
            Tone.AddToneData(@"(しよう|すべし|しましょう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しようよ！$2");
            Tone.AddToneData(@"(じますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"じるかな！$2");
            Tone.AddToneData(@"(じる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"じるね！$2");
            Tone.AddToneData(@"(じます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"じるよ！$2");
            Tone.AddToneData(@"(しんで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"しんでね！$2");
            Tone.AddToneData(@"(すべて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"すべてだね！$2");
            Tone.AddToneData(@"(しますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するかな～$2");
            Tone.AddToneData(@"(しますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するけど！$2");
            Tone.AddToneData(@"(します|シマス|しますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するね～$2");
            Tone.AddToneData(@"(すべしか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するべきかな～$2");
            Tone.AddToneData(@"(する|しますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するよ！$2");
            Tone.AddToneData(@"(しますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"するね！$2");
            Tone.AddToneData(@"(ぜず)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せずね！$2");
            Tone.AddToneData(@"(せました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せたよ～$2");
            Tone.AddToneData(@"(ぜました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぜたよ～$2");
            Tone.AddToneData(@"(せません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せないよ！$2");
            Tone.AddToneData(@"(せますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるかな～$2");
            Tone.AddToneData(@"(せますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるね！$2");
            Tone.AddToneData(@"(せる|せます|せますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるよ！$2");
            Tone.AddToneData(@"(せますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"せるね！$2");
            Tone.AddToneData(@"(そうで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"そうだね！$2");
            Tone.AddToneData(@"(たので)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たからね！$2");
            Tone.AddToneData(@"(なので|ですから)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だからね！$2");
            Tone.AddToneData(@"(だけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だけだよ！$2");
            Tone.AddToneData(@"(ですが|ですがね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だけどね！$2");
            Tone.AddToneData(@"(だけに)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だけにね！$2");
            Tone.AddToneData(@"(ですし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だしね！$2");
            Tone.AddToneData(@"(たそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たそうだね～$2");
            Tone.AddToneData(@"(たち)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たちだね！$2");
            Tone.AddToneData(@"(でたしね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だったね！$2");
            Tone.AddToneData(@"(だって)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だってね！$2");
            Tone.AddToneData(@"(だと)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だと！$2");
            Tone.AddToneData(@"(ちません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たないよ！$2");
            Tone.AddToneData(@"(た|ましたな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たね～$2");
            Tone.AddToneData(@"(だ|だな|なり|ですな|ですね|っすね|ッスね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だね～$2");
            Tone.AddToneData(@"(たのか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たのかな！$2");
            Tone.AddToneData(@"(たぶん)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たぶんね！$2");
            Tone.AddToneData(@"(あかんか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だめかぁ$2");
            Tone.AddToneData(@"(ため)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ためだね！$2");
            Tone.AddToneData(@"(あかんわ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ダメだね！$2");
            Tone.AddToneData(@"(ために)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ためにね！$2");
            Tone.AddToneData(@"(たぜ|たわ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たよ～$2");
            Tone.AddToneData(@"(いです)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"いよね～$2");
            Tone.AddToneData(@"(だぜ|のだ|だわ|です|ですよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だよ！$2");
            Tone.AddToneData(@"(だろ|だよなぁ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だよね！$2");
            Tone.AddToneData(@"(だらけ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だらけだね～$2");
            Tone.AddToneData(@"(たらで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たらだね～$2");
            Tone.AddToneData(@"(たら)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たらね！$2");
            Tone.AddToneData(@"(たりで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たりだね！$2");
            Tone.AddToneData(@"(たる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"たるよ$2");
            Tone.AddToneData(@"(だろうが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だろうけどね$2");
            Tone.AddToneData(@"(しょうね|だろうなぁ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"だろうね！$2");
            Tone.AddToneData(@"(断じて無い|断じてねぇ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"断じて無いよ！$2");
            Tone.AddToneData(@"(おくれ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"おくれよ～$2");
            Tone.AddToneData(@"(ちらで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ちらだよ！$2");
            Tone.AddToneData(@"(ちます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ちるよ！$2");
            Tone.AddToneData(@"(ついて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ついてだね～$2");
            Tone.AddToneData(@"(づく)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"づくね！$2");
            Tone.AddToneData(@"(作ろう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"作ろうよ！$2");
            Tone.AddToneData(@"(つくろう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"つくろうよ！$2");
            Tone.AddToneData(@"(ちました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ったよ！$2");
            Tone.AddToneData(@"(ていう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ていう感じだね！$2");
            Tone.AddToneData(@"(てく)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ていくね～$2");
            Tone.AddToneData(@"(てきて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てきてね～$2");
            Tone.AddToneData(@"(できるか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"できるかな～$2");
            Tone.AddToneData(@"(でしょう|でしょうか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょ！$2");
            Tone.AddToneData(@"(だろう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でしょうね！$2");
            Tone.AddToneData(@"(てました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てたよ～$2");
            Tone.AddToneData(@"(でました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でたよ～$2");
            Tone.AddToneData(@"(出ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"出たよ！$2");
            Tone.AddToneData(@"(てません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てないよ！$2");
            Tone.AddToneData(@"(でません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でないよ！$2");
            Tone.AddToneData(@"(出ません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"出ないよ！$2");
            Tone.AddToneData(@"(てもよか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てもいいかな～$2");
            Tone.AddToneData(@"(でもよか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でもいいかな～$2");
            Tone.AddToneData(@"(でるか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でるかかな！$2");
            Tone.AddToneData(@"(てますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるかな！$2");
            Tone.AddToneData(@"(出るか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"出るかな！$2");
            Tone.AddToneData(@"(てますが|テますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるけど$2");
            Tone.AddToneData(@"(てるし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるしね！$2");
            Tone.AddToneData(@"(てる|てますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるね！$2");
            Tone.AddToneData(@"(でる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でるね！$2");
            Tone.AddToneData(@"(てます|てますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるよ～$2");
            Tone.AddToneData(@"(でます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"でるよ～$2");
            Tone.AddToneData(@"(てますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"てるわね！$2");
            Tone.AddToneData(@"(という)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"というらしいね～$2");
            Tone.AddToneData(@"(とおり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"とおりだね～$2");
            Tone.AddToneData(@"(とき)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ときだね！$2");
            Tone.AddToneData(@"(とな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"とね～$2");
            Tone.AddToneData(@"(とは)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"とはね！$2");
            Tone.AddToneData(@"(ませんか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないかな～$2");
            Tone.AddToneData(@"(内緒|ないしょ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないしょだよ～$2");
            Tone.AddToneData(@"(ないで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないでね！$2");
            Tone.AddToneData(@"(なくて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なくてね～$2");
            Tone.AddToneData(@"(なって)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なってね～$2");
            Tone.AddToneData(@"(など)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"などだよ～$2");
            Tone.AddToneData(@"(なのか|であると)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なのかな～$2");
            Tone.AddToneData(@"(なさいよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なよ！$2");
            Tone.AddToneData(@"(なる|なるわ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なるよ！$2");
            Tone.AddToneData(@"(にっこり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"にっこりだね～$2");
            Tone.AddToneData(@"(にて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"にてね！$2");
            Tone.AddToneData(@"(には)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"にはね！$2");
            Tone.AddToneData(@"(にます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"にますね～$2");
            Tone.AddToneData(@"(なぁ|わぁ|やぁ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"なぁ$2");
            Tone.AddToneData(@"(ねました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ねたよ！$2");
            Tone.AddToneData(@"(寝ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"寝たよ！$2");
            Tone.AddToneData(@"(ねて)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ねてね！$2");
            Tone.AddToneData(@"(ねません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ねないよ！$2");
            Tone.AddToneData(@"(寝ません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"寝ないよ！$2");
            Tone.AddToneData(@"(ねます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ねるね！$2");
            Tone.AddToneData(@"(やら)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のかな！$2");
            Tone.AddToneData(@"(のみ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のみね！$2");
            Tone.AddToneData(@"(のよう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"のようだね！$2");
            Tone.AddToneData(@"(映ません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"映えないよ！$2");
            Tone.AddToneData(@"(はず)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"はずだよ！$2");
            Tone.AddToneData(@"(ばす)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ばすだね～$2");
            Tone.AddToneData(@"(ばかり|ばっかり)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ばっかりだね！$2");
            Tone.AddToneData(@"(びません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ばないよ～$2");
            Tone.AddToneData(@"(びました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"びたよ！$2");
            Tone.AddToneData(@"(びますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"びるかな！$2");
            Tone.AddToneData(@"(びる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"びるね！$2");
            Tone.AddToneData(@"(びます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぶよ！$2");
            Tone.AddToneData(@"(ぶる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぶるね！$2");
            Tone.AddToneData(@"(べき)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べきだよ！$2");
            Tone.AddToneData(@"(べし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べしだね！$2");
            Tone.AddToneData(@"(べました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べたよ！$2");
            Tone.AddToneData(@"(べよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べてよ！$2");
            Tone.AddToneData(@"(べますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べるかな～$2");
            Tone.AddToneData(@"(べます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"べるよ～$2");
            Tone.AddToneData(@"(ほど)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ほどかな～$2");
            Tone.AddToneData(@"(ぼる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ぼるよ！$2");
            Tone.AddToneData(@"(ませんな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ませんな～$2");
            Tone.AddToneData(@"(まって)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"まってね$2");
            Tone.AddToneData(@"(まで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"までだね～$2");
            Tone.AddToneData(@"(みません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"まないよ！$2");
            Tone.AddToneData(@"(まる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"まるね～$2");
            Tone.AddToneData(@"(みました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みたよ！$2");
            Tone.AddToneData(@"(見ました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"見たよ！$2");
            Tone.AddToneData(@"(みよう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みようよ！$2");
            Tone.AddToneData(@"(みますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みるかな！$2");
            Tone.AddToneData(@"(みる|みますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みるね！$2");
            Tone.AddToneData(@"(みます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みるよ！$2");
            Tone.AddToneData(@"(みますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"みるわね！$2");
            Tone.AddToneData(@"(めました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"めたよ！$2");
            Tone.AddToneData(@"(めません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"めないよ～$2");
            Tone.AddToneData(@"(める|めます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"めるよ！$2");
            Tone.AddToneData(@"(もよし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もいいね～$2");
            Tone.AddToneData(@"(もいう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もいうね！$2");
            Tone.AddToneData(@"(も)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もだね～$2");
            Tone.AddToneData(@"(ものか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ものかな！$2");
            Tone.AddToneData(@"(もの)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ものね～$2");
            Tone.AddToneData(@"(もらう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"もらうよ！$2");
            Tone.AddToneData(@"(やっちゃう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"やっちゃうよ！$2");
            Tone.AddToneData(@"(やれ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"やってね！$2");
            Tone.AddToneData(@"(やる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"やるよ！$2");
            Tone.AddToneData(@"(やりましょう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"やろうよ！$2");
            Tone.AddToneData(@"(よな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"よね！$2");
            Tone.AddToneData(@"(よろしう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"よろしくね～$2");
            Tone.AddToneData(@"(だそう|のだとか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"らしいね～$2");
            Tone.AddToneData(@"(らず)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"らずだね～$2");
            Tone.AddToneData(@"(りません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"らないよ～$2");
            Tone.AddToneData(@"(らまで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"らまでだよ～$2");
            Tone.AddToneData(@"(りそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"りそうだね～$2");
            Tone.AddToneData(@"(りました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"りたよ！$2");
            Tone.AddToneData(@"(りますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"りるね！$2");
            Tone.AddToneData(@"(りますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"りるよ！$2");
            Tone.AddToneData(@"(りますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るかな！$2");
            Tone.AddToneData(@"(りますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るけど！$2");
            Tone.AddToneData(@"(るそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るそうだね！$2");
            Tone.AddToneData(@"(るな|ますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るね！$2");
            Tone.AddToneData(@"(るのか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るのかな！$2");
            Tone.AddToneData(@"(るぜ|ります|ますよ)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るよ！$2");
            Tone.AddToneData(@"(りますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"るわね～$2");
            Tone.AddToneData(@"(れそう)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れそうだね～$2");
            Tone.AddToneData(@"(れました)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れたよ！$2");
            Tone.AddToneData(@"(れで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れでね！$2");
            Tone.AddToneData(@"(れません)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れないよう～$2");
            Tone.AddToneData(@"(れず)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れなかったね！$2");
            Tone.AddToneData(@"(れますか)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるかな～$2");
            Tone.AddToneData(@"(れますが)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるけど！$2");
            Tone.AddToneData(@"(れる|れますね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるね！$2");
            Tone.AddToneData(@"(れます|れマス)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるよ！$2");
            Tone.AddToneData(@"(れますな)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"れるわね！$2");
            Tone.AddToneData(@"(わからん)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わからないね！$2");
            Tone.AddToneData(@"(わる)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"わるよ！$2");
            Tone.AddToneData(@"(のでは)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"んじゃないかな！$2");
            Tone.AddToneData(@"(のね|んやで|ンやで)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"んだね！$2");
            Tone.AddToneData(@"(ねや)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"んでよ！$2");
            Tone.AddToneData(@"(覚ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"覚ますよ！$2");
            Tone.AddToneData(@"(居ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"居るよ！$2");
            Tone.AddToneData(@"(見ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"見るよ！$2");
            Tone.AddToneData(@"(出ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"出るよ！$2");
            Tone.AddToneData(@"(寝ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"寝るね！$2");
            Tone.AddToneData(@"(来ます)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"来るよ！$2");
            Tone.AddToneData(@"([\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}\p{IsCJKUnifiedIdeographsExtensionA}]|[\uD840-\uD869][\uDC00-\uDFFF]|\uD869[\uDC00-\uDEDF])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1だね～$2");
            Tone.AddToneData(@"([ア-ヴ])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1だね～$2");
            Tone.AddToneData(@"([0-9])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1だね～$2");
            Tone.AddToneData(@"([０-９])(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"$1だね～$2");
            Tone.AddToneData(@"(おり)(、)", @"いて$2");
            Tone.AddToneData(@"(あり)(、)", @"あって$2");
            Tone.AddToneData(@"(が)(、)", @"けど$2");
            Tone.AddToneData(@"(ですけどね|ですけど)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"けどね$2");
            Tone.AddToneData(@"(ですよね)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"よね$2");
            Tone.AddToneData(@"(ねえし)(。|｡|．|！|!|？|\?|\)|』|…|―|$)", @"ないよ$2");

            return Tone;
        }
    }
}
