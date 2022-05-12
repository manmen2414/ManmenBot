using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Manmenbot
{
    class LoopClass
    {
        public static void LoopMain()
        {
            MenuCheck();
        }

        public static async Task MenuCheck()
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.KeyChar == 'm')
                {
                    DiscordSocketClient _client = Main.Program._client;
                    Console.Write("\r\nメニュー - 動作を入力してください。");
                    Console.Write("\r\n[終了,メッセージ送信]");
                    string CR = Console.ReadLine();
                    switch (CR)
                    {
                        case "終了":
                            await _client.SetStatusAsync(UserStatus.Offline);
                            var embed = new EmbedBuilder()
                            .WithTitle("**終了**") // 終了
                            .WithDescription($"{_client.CurrentUser.Username}が終了しました。") // (bot名)が終了しました。
                            .WithColor(Color.DarkRed) //サイドの色を設定
                            .WithCurrentTimestamp() // 実行日時を追加
                            .Build(); // ビルド
                            await (_client.GetChannel(Manmenbot.Settngs.notificationNotTextChannel) as SocketTextChannel).SendMessageAsync(null, false, embed);
                            Environment.Exit(0);
                            break;
                        case "メッセージ送信":
                            Console.Write("\r\n送信するチャンネルID:");
                            ulong id = ulong.Parse(Console.ReadLine());
                            Console.Write("\r\n送信する内容:");
                            string message = Console.ReadLine();
                            await (_client.GetChannel(id) as SocketTextChannel).SendMessageAsync(message + "[送信:bot管理者]");
                            break;

                    }
                }
                await Task.Delay(200);
                //1000 = 1s この場合0.2秒に1回
            }
        }
    }
}
