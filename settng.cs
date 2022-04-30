using System;
using Discord;
using Discord.WebSocket;

namespace Settng
{
    class Settngs
    {
        public static readonly SocketUser hostuser = (Main.Program._client.GetUser(778582802504351745));
        //あなたの垢のID入れてね(終了コマンドなどの専用コマンドが使えるようになります)
        public static readonly string path = Environment.GetEnvironmentVariable("botpach", EnvironmentVariableTarget.User);
        //csファイルがあるフォルダーまでのフルパスを入れてください(ここでは環境変数を使ってますが普通にパス入れるときは
        //public static string path = "パス"　でお願いします。)
        public static readonly string token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.User);
        //token入れてください(ここも環境変数使ってるので上と同じようにしてtokenを入れてください)
        public static readonly SocketTextChannel notificationChannel = (Main.Program._client.GetChannel(947034727452389397)) as SocketTextChannel;
        public static readonly bool notificationcheck = false;
        //上側:GetChannel(947034727452389397)の()にチャンネルID入れる事で起動終了通知のチャンネルが設定できる
        //下側:trueをfalseにすれば通知しなくなる
    }   
}