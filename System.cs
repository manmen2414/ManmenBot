using Discord;
using Discord.WebSocket;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manmenbot
{
    class Commands
    {
        public static DiscordSocketClient _client = Main.Program._client;
        public static async Task Help(SocketMessage message)//Helpコマンド。Embedを使用。
        {
            var embed = new EmbedBuilder() // Embedのビルダーを宣言……面倒なのでBuild()まで含めてしまいます。
           .WithAuthor(message.Author.Username,  // ここでユーザー名とアイコンを取得
            message.Author.GetAvatarUrl() ?? message.Author.GetDefaultAvatarUrl())
           .WithImageUrl("https://discord.com/channels/934335666244943872/934335666244943875/961925436626063370")
           .WithTitle("まんめんbot") // タイトルを設定
           .WithDescription("まんめんbotのコマンドは以下の通りです。") // 説明を設定
           .WithColor(Color.DarkGreen) //サイドの色を設定
           .AddField("m?こんにちは", "[こんにちは！(名前)さん！]を返す", true)
           .AddField("m?サイコロ", "サイコロを振る。", true)
           .AddField("m?おみくじ", "おみくじを引く。", true)
           .AddField("m?時間/m?時間秒", "時間を取得します。時間秒では秒まで取得します。", true)
           .AddField("gg", "[gg!]を返す", true)
           .AddField("m?ランダム (~100 確率)", "確率で当たったり外れたりします。(確率>50だと信頼なし)", true)
           .AddField("m?計算 (数値1),(+ or - or * × or / ÷),(数値2)", "計算する。", true)
           .AddField("(+ or - or =)(数値)", "カウンターします。+が足し-が引き=がセット", true)
           .AddField("m?VCM", "IDで指定したVCのメンション。", true)
           .AddField("m?じゃんけん (パー or グー or チョキ)", "じゃんけんできるだけ。", true)
           .AddField("m?おやすみ", "[お…や…す……( ˘ω˘ ) ｽﾔｧ…]を返す。", true)
           .WithCurrentTimestamp() // 実行日時を追加
           .Build(); // ビルド
            await message.Channel.SendMessageAsync(null, false, embed);
        }

        public static async Task Konnitiha(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("こんにちは、" + message.Author.Username + "さん！");
        }

        public static async Task Saikoro(SocketMessage message)
        {
            if (Main.Program.okcommand[0])
            {
                Random r = new Random();
                await message.Channel.SendMessageAsync("1~6のサイコロ、結果は" + r.Next(1, 7).ToString() + "です。");
            }
        }

        public static async Task Block(SocketMessage message)
        {
            if (Main.Program.okcommand[1])
            {
                Random r = new Random();
                string block = Enum.GetName(typeof(blocks), r.Next(0, 60));
                await message.Channel.SendMessageAsync(block);
            }
        }

        public static async Task Omikuji(SocketMessage message)
        {
            if (Main.Program.okcommand[2])
            {
                Random r = new Random();
                string omiku = Enum.GetName(typeof(omikuji), r.Next(0, 7));
                await message.Channel.SendMessageAsync("**" + omiku + "**");
            }
        }

        public static async Task Time(SocketMessage message)
        {
            if (Main.Program.okcommand[3])
            {
                DateTime dt = DateTime.Now;
                switch (message.ToString())
                {
                    case "m?時間":
                        await message.Channel.SendMessageAsync("現在時刻は" + dt.ToString("yyyy年MM月dd日 HH時mm分ss秒" + "です。"));
                        break;
                    case "m?時間秒":
                        await message.Channel.SendMessageAsync("現在時刻は" + dt.ToString("yyyy年MM月dd日 HH時mm分" + "です。"));
                        break;
                }
            }
        }

        public static async Task End(SocketMessage message)
        {
            if (message.Author.Id == Manmenbot.Settngs.hostuser)
            {
                var embed = new EmbedBuilder()
                .WithTitle("**終了**") // 終了
                .WithDescription($"{_client.CurrentUser.Username}が終了しました。") // (bot名)が終了しました。
                .WithColor(Color.DarkRed) //サイドの色を設定
                .WithCurrentTimestamp() // 実行日時を追加
                .Build(); // ビルド
                await (_client.GetChannel(Manmenbot.Settngs.notificationNotTextChannel) as SocketTextChannel).SendMessageAsync(null, false, embed);
                await _client.SetStatusAsync(UserStatus.Offline);
                Environment.Exit(0);
            }
            else
            {
                await message.Channel.SendMessageAsync($"{_client.GetUser(Manmenbot.Settngs.hostuser).Username}以外は終了できません。");
            }
        }

        public static async Task Cal(SocketMessage message)
        {
            if (Main.Program.okcommand[4])
            {
                string[] arr = message.ToString().Split(' ');
                string[] arra = arr[1].Split('+', '-', '*', '/', '×', '÷');
                string siki = arr[1];
                char enzansi = siki[arra[0].Length];
                double med = double.Parse(arra[0]);
                double med2 = double.Parse(arra[1]);
                double med3 = 0;
                switch (enzansi)
                {
                    case '+':
                        med3 = med + med2;
                        break;
                    case '-':
                        med3 = med - med2;
                        break;
                    case '*':
                        med3 = med * med2;
                        break;
                    case '×':
                        med3 = med * med2;
                        break;
                    case '/':
                        med3 = med / med2;
                        break;
                    case '÷':
                        med3 = med / med2;
                        break;
                    default:
                        await message.Channel.SendMessageAsync("演算子は + - / * または + - × ÷ である必要があります。");
                        break;
                }
                await message.Channel.SendMessageAsync("答えは**" + med3.ToString() + "**です。");
            }
        }

        public static async Task Randam(SocketMessage message)
        {
            string[] arr = message.ToString().Split(' ');
            float med = float.Parse(arr[1]);
            if (med < 100 && Main.Program.okcommand[5])
            {
                int med2 = (int)(100 / med);
                Random r = new Random();
                if (r.Next(0, med2) == 0)
                {
                    await message.Channel.SendMessageAsync($"{med}%の確率ランダム、結果は**当たり**です。");
                }
                else
                {
                    await message.Channel.SendMessageAsync($"{med}%の確率ランダム、結果は**はずれ**です。");
                }
            }
        }
        public static async Task Janken(SocketMessage message)
        {
            if (Main.Program.okcommand[6])
            {
                string[] arr = message.ToString().Split(' ', StringSplitOptions.None);
                string[] janken = { "グー", "チョキ", "パー" };
                Random r = new Random();
                string jan = janken[r.Next(0, 3)];
                string goal = null;
                goal = "このコードが表示されたら、お知らせください";
                if (arr[1] != "グー" && arr[1] != "チョキ" && arr[1] != "パー")
                {
                    await message.Channel.SendMessageAsync($"**超エラー！**\r\n引数が間違っています。");
                }
                else
                {
                    if (jan == arr[1])//あいこ
                    {
                        goal = "あいこ";
                    }
                    else
                    {
                        switch (arr[1])
                        {
                            case "グー":
                                switch (jan)
                                {
                                    case "パー":
                                        goal = "負け";
                                        break;
                                    case "チョキ":
                                        goal = "勝ち";
                                        break;
                                }
                                break;
                            case "チョキ":
                                switch (jan)
                                {
                                    case "グー":
                                        goal = "負け";
                                        break;
                                    case "パー":
                                        goal = "勝ち";
                                        break;
                                }
                                break;
                            case "パー":
                                switch (jan)
                                {
                                    case "チョキ":
                                        goal = "負け";
                                        break;
                                    case "グー":
                                        goal = "勝ち";
                                        break;
                                }
                                break;
                        }
                    }
                    await message.Channel.SendMessageAsync($"{message.Author.Mention}→{arr[1]}　　{_client.CurrentUser.Mention}→{jan} \r\n {goal}です。");
                }
            }
        }

        public static async Task<long> Caunter(SocketMessage message, long me)
        {
            string[] arr = message.ToString().Split('+', '-', '=');
            char chr = message.ToString()[0];
            switch (chr)
            {
                case '+':
                    me = me + int.Parse(arr[1]);
                    break;
                case '-':
                    me = me - int.Parse(arr[1]);
                    break;
                case '=':
                    me = int.Parse(arr[1]);
                    break;
            }
            await message.Channel.SendMessageAsync("カウンター:**" + me.ToString() + "**");
            return me;
        }

        public static async Task QuickRole(SocketMessage message)
        {
            if (Main.Program.IsDMmessage(message))
            {
                await message.Channel.SendMessageAsync("このコマンドはDMで利用できません。");
            }
            else
            {
                string[] arr = message.ToString().Split(' ');
                await Main.Program.scktgid.CreateRoleAsync(arr[1], GuildPermissions.None, Color.DarkRed, false, RequestOptions.Default);
                await message.Channel.SendMessageAsync("ロールを生成しました。");
            }
        }

        public static async Task Joke_Oyasumi(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("お…や…す……( ˘ω˘ ) ｽﾔｧ");
        }
    }
}