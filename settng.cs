using System;
using Discord;
using Discord.WebSocket;

namespace Settng
{
    class Settngs
    {
        public static readonly ulong hostuser = 778582802504351745;
        //あなたの垢のID入れてね(終了コマンドなどの専用コマンドが使えるようになります)
        public static readonly string path = Environment.GetEnvironmentVariable("botpach", EnvironmentVariableTarget.User);
        //csファイルがあるフォルダーまでのフルパスを入れてください(ここでは環境変数を使ってますが普通にパス入れるときは
        //public static string path = "パス"　でお願いします。)
        public static readonly string token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.User);
        //token入れてください(ここも環境変数使ってるので上と同じようにしてtokenを入れてください)
        public static readonly ulong notificationChannel = 947034727452389397;
        public static readonly bool notificationcheck = false;
        //上側:947034727452389397の()にチャンネルID入れる事で起動終了通知のチャンネルが設定できる
        //下側:trueをfalseにすれば通知しなくなる
        public static readonly string[] notificationmessage = { $"`スタート:まんめんbot({DateTime.Now})`", $"`終了:まんめんbot({DateTime.Now})`" };
        //スタート時と終了時のメッセージをいじれる。
    }   
}
