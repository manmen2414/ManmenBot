using System;
using Discord;
using Discord.WebSocket;

namespace Manmenbot
{
    class Settngs
    {
        public static DiscordSocketClient _client = new DiscordSocketClient();
        public static readonly ulong hostuser = 778582802504351745;
        //管理者のID入れて(終了コマンドなどの専用コマンドが使えるようになります)
        public static readonly string path = Environment.GetEnvironmentVariable("botpach", EnvironmentVariableTarget.User);
        //csファイルがあるフォルダーまでのフルパスを入れてください(ここでは環境変数を使ってますが普通にパス入れるときは
        //public static string path = "パス"　でお願いします。)
        public static readonly string token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.User);
        //token入れてください(ここも環境変数使ってるので上と同じようにしてtokenを入れてください)
        public static readonly ulong notificationNotTextChannel = 967296101134774330;
        //947034727452389397にチャンネルID入れる事で起動終了通知のチャンネルが設定できる
        public static readonly bool notificationcheck = true;
        //通知するかしないか(true:する,false:しない)
        public static readonly bool[] commandswich = { true, true, true, true, true, true, true, true, true };
        //有効なコマンドをいじる。なお m?設定 でいじれる模様。 サイコロ,ブロック,おみくじ,時間,カウンター,ランダム,
    }
}
