using Discord;
using Discord.WebSocket;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public enum blocks
{
    石,
    閃緑岩,
    磨かれた閃緑岩,
    花崗岩,
    磨かれた花崗岩,
    丸石,
    苔石,
    黒曜石,
    石レンガ,
    苔石レンガ,
    ひび割れた石レンガ,
    模様入りの石レンガ,
    滑らかな石,
    滑らかな石英,
    土,
    粗い土,
    ポドソル,
    菌糸,
    アカシアの木材,
    オークの木材,
    ジャングルの木材,
    シラカバの木材,
    ダークオークの木材,
    マツの木材,
    オークの原木,
    ジャングルの原木,
    シラカバの原木,
    マツの原木,
    アカシアの木材のハーフブロック,
    オークの木材のハーフブロック,
    ジャングルの木材のハーフブロック,
    シラカバの木材のハーフブロック,
    ダークオークの木材のハーフブロック,
    マツの木材のハーフブロック,
    マツの木材の階段,
    シラカバの木材の階段,
    ジャングルの木材の階段,
    アカシアの原木,
    ダークオークの原木,
    アカシアの木材の階段,
    ダークオークの木材の階段,
    オークの木材の階段,
    ダークオークの木,
    アカシアの木,
    ジャングルの木,
    シラカバの木,
    マツの木,
    オークの木,
    樹皮を剥いだダークオークの木,
    樹皮を剥いだアカシアの木,
    樹皮を剥いだジャングルの木,
    樹皮を剥いだシラカバの木,
    樹皮を剥いだマツの木,
    樹皮を剥いだオークの木,
    樹皮を剥いだオークの原木,
    樹皮を剥いだマツの原木,
    樹皮を剥いだシラカバの原木,
    樹皮を剥いだジャングルの原木,
    樹皮を剥いだアカシアの原木,
    樹皮を剥いだダークオークの原木
}
public enum omikuji
{
    大凶,
    凶,
    未吉,
    吉,
    小吉,
    中吉,
    大吉
}

namespace Main
{
    class Program
    {
        int me = 0;
        bool[] bol = { true, true, true, true, true, true, true, true, true };
        ulong[] ulgkakko = { 0, 0, 0 };
        public static DiscordSocketClient _client;
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }
        public Program()
        {
            _client = new DiscordSocketClient();
            _client.Log += LogAsync;
            _client.Ready += onReady;
            _client.MessageReceived += OnMessage;
            _client.Disconnected += dis;
        }

        public async Task MainAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Settng.Settngs.token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            writeline($"{DateTime.Now:yyyy/MM/dd}{log}");
            return Task.CompletedTask;
        }

        public async Task onReady()
        {
            Console.WriteLine($"[{_client.CurrentUser.Username}]botは接続されました");
            SocketTextChannel nc = _client.GetChannel(Settng.Settngs.notificationChannel) as SocketTextChannel;
            if (nc == null)
            {
                Console.WriteLine("エラー:notificationChannelがnullです(入力値が間違っている可能性があります)");
            }
            else
            {
                if (Settng.Settngs.notificationcheck)
                    await nc.SendMessageAsync(Settng.Settngs.notificationmessage[0]);
            }
        }

        private async Task OnMessage(SocketMessage message)
        {
            SocketGuild scktgid = null;
            if (IsDMmessage(message))
            {
                writeline($"{DateTime.Now} {message.Author.Username}({message.Channel.ToString()})|=|{message.ToString()}({message.Author.Id})");
            }
            else
            {
                scktgid = (message.Channel as SocketGuildChannel).Guild;
                writeline($"{DateTime.Now} {message.Author.Username}({scktgid.Name})|=|{message}({message.Author.Id})");
            }

            if (message.Author.Id == _client.CurrentUser.Id || message.Author.Id == ulgkakko[0] || message.Author.Id == ulgkakko[1] || message.Author.Id == ulgkakko[2])
            {
                return;
            }
            else
            {
                if (message.Content == "m?help")
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
                if (message.Content == "m?こんにちは")//こんにちは
                {
                    await message.Channel.SendMessageAsync("こんにちは、" + message.Author.Username + "さん！");
                }
                if (message.Content == "m?サイコロ" && bol[0])//サイコロ
                {
                    Random r = new Random();
                    await message.Channel.SendMessageAsync("1~6のサイコロ、結果は" + r.Next(1, 7).ToString() + "です。");
                }
                if (message.Content == "m?ブロック" && bol[1])//ランダムブロック
                {
                    Random r = new Random();
                    string block = Enum.GetName(typeof(blocks), r.Next(0, 60));
                    await message.Channel.SendMessageAsync(block);
                }
                if (message.Content == "m?おみくじ" && bol[2])//おみくじ
                {
                    Random r = new Random();
                    string omiku = Enum.GetName(typeof(omikuji), r.Next(0, 7));
                    await message.Channel.SendMessageAsync("**" + omiku + "**");
                }
                if (message.Content == "m?時間" && bol[3])//おみくじ
                {
                    DateTime dt = DateTime.Now;
                    await message.Channel.SendMessageAsync("現在時刻は" + dt.ToString("yyyy年MM月dd日 HH時mm分" + "です。"));
                }
                if (message.Content == "m?時間秒" && bol[3])//おみくじ
                {
                    DateTime dt = DateTime.Now;
                    await message.Channel.SendMessageAsync("現在時刻は" + dt.ToString("yyyy年MM月dd日 HH時mm分ss秒" + "です。"));
                }
                if (message.Content == "gg" && bol[4])//おみくじ
                {
                    await message.Channel.SendMessageAsync("gg!");
                }
                if (message.ToString().StartsWith("m?ランダム") && bol[5])
                {
                    string[] del = { "m?ランダム " };
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    double med = double.Parse(arr[1]);
                    if (med < 100)
                    {
                        int med2 = (int)(100 / med);
                        Random r = new Random();
                        if (r.Next(0, med2) == 0)
                        {
                            await message.Channel.SendMessageAsync($"{med}%の確率ランダム、結果は**当たり**です。");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync($"{med}の確率ランダム、結果は**はずれ**です。");
                        }
                    }
                    else
                    {
                        if (med.ToString() == null)
                        {
                            await message.Channel.SendMessageAsync("ここで入力する文字は99以下の数字である必要があります。");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("ここで入力する数は99以下である必要があります。");
                        }

                    }
                }
                if (message.ToString().StartsWith("m?計算") && bol[6])
                {
                    string[] del = { "m?計算 " };
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    string[] arra = arr[1].Split(',');
                    double med = double.Parse(arra[0]);
                    double med2 = double.Parse(arra[2]);
                    if (arra[1] == "+")
                    {
                        double med3 = med + med2;
                        await message.Channel.SendMessageAsync("答えは**" + med3.ToString() + "**です。");
                    }
                    else
                    {
                        if (arra[1] == "-")
                        {
                            double med3 = med - med2;
                            await message.Channel.SendMessageAsync("答えは**" + med3.ToString() + "**です。");
                        }
                        else
                        {
                            if (arra[1] == "*" || arra[1] == "×")
                            {
                                double med3 = med * med2;
                                await message.Channel.SendMessageAsync("答えは**" + med3.ToString() + "**です。");
                            }
                            else
                            {
                                if (arra[1] == "/" || arra[1] == "÷")
                                {
                                    double med3 = med / med2;
                                    await message.Channel.SendMessageAsync("答えは**" + med3.ToString() + "**です。");
                                }
                                else
                                {
                                    await message.Channel.SendMessageAsync("演算子は + - * / か + - × ÷ である必要があります。");
                                }
                            }
                        }
                    }
                }
                if (message.ToString().StartsWith("m?割合変換"))//「m?割合変換」で始まる
                {
                    string[] del = { "m?割合変換 " };//「m?割合変換」消し
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    Console.WriteLine(arr[1]);
                    string[] del2 = { "分の" };//arr2[0]分のarr2[1]
                    string[] arr2 = arr[1].ToString().Split(del2, StringSplitOptions.None);
                    if (arr2[0] == null)//何分の何じゃない場合
                    {
                        Console.WriteLine("ar000");
                        string[] del3 = { "%" };//何分の何じゃないんだったら%で割るしかないじゃねえか！！！
                        string[] arr3 = arr[1].ToString().Split(del3, StringSplitOptions.None);
                        if (arr3[0] == null)
                        {
                            await message.Channel.SendMessageAsync("引数は[m?割合変換 00%]か[m?割合変換 0分の0]である必要があります。");//超エラー
                        }
                        int bu2;
                        int bu1;
                        int med = int.Parse(arr3[0]);
                        if (med > 50)
                        {
                            med = med - 50;
                            bu2 = med - 1;
                        }
                        else
                        {
                            bu2 = 1;
                        }
                        bu1 = 100 / med;
                        await message.Channel.SendMessageAsync(arr[3].ToString() + "%は" + bu1.ToString() + "分の" + bu2.ToString() + "です。");
                    }
                    else
                    {

                    }

                }
                if (message.Content == "m?debug:num1")
                {
                    var insuu = _client.GetChannel(947034727452389397) as SocketTextChannel;
                    Console.WriteLine(insuu.ToString());
                }
                if (message.ToString().StartsWith("+"))
                {
                    string[] del = { "+" };
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    Console.WriteLine(arr[1]);
                    me = me + int.Parse(arr[1]);
                    await message.Channel.SendMessageAsync("カウンター:**" + me.ToString() + "**");
                }
                if (message.ToString().StartsWith("-"))
                {
                    string[] del = { "-" };
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    Console.WriteLine(arr[1]);
                    me = me - int.Parse(arr[1]);
                    await message.Channel.SendMessageAsync("カウンター:**" + me.ToString() + "**");
                }
                if (message.ToString().StartsWith("="))
                {
                    string[] del = { "=" };
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    Console.WriteLine(arr[1]);
                    me = int.Parse(arr[1]);
                    await message.Channel.SendMessageAsync("カウンター:**" + me.ToString() + "**");
                }
                if (message.Content == "m?終了")
                {
                    if (message.Author.Id == Settng.Settngs.hostuser)
                    {
                        SocketTextChannel nc = _client.GetChannel(Settng.Settngs.notificationChannel) as SocketTextChannel;
                        await nc.SendMessageAsync(Settng.Settngs.notificationmessage[1]);
                        await _client.StopAsync().ConfigureAwait(false);
                        Environment.Exit(0);

                    }
                    else
                    {
                        await message.Channel.SendMessageAsync($"{_client.GetUser(Settng.Settngs.hostuser).Username}以外は終了できません。");
                    }
                }
                if (message.ToString().StartsWith("m?VCM") && bol[7])//いらない子と化してるVCM
                {
                    string[] del = { "m?VCM " };
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    ulong s = ulong.Parse(arr[1]);
                    var insuu = _client.GetChannel(s) as SocketVoiceChannel;
                    string test = insuu.Mention;
                    await message.Channel.SendMessageAsync(message.Author.Username + "より: " + test);
                    await message.DeleteAsync();
                }
                await mess2(message, scktgid);
            }
        }

        private Task dis(Exception exc)
        {
            Console.WriteLine("!警告 : botは切断されました \r\n原因:" + exc);
            return Task.CompletedTask;
        }
        public async Task mess2(SocketMessage message, SocketGuild scktgid)
        {
            if (message.ToString().StartsWith("m?クイックロール"))
            {
                if (IsDMmessage(message))
                {
                    await message.Channel.SendMessageAsync("このコマンドはDMで利用できません。");
                }
                else
                {
                    string[] arr = message.ToString().Split(' ');
                    await scktgid.CreateRoleAsync(arr[1], GuildPermissions.None, Color.DarkRed, false, RequestOptions.Default);
                    var role = scktgid.Roles.FirstOrDefault(x => x.Name == "test");
                    await scktgid.GetUser(message.Author.Id).AddRoleAsync(role);
                }
            }
            if (message.ToString().StartsWith("m?じゃんけん") && bol[8])
            {
                string[] del = { "m?じゃんけん " };
                string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                string[] janken = { "グー", "チョキ", "パー" };
                Random r = new Random();
                string jan = janken[r.Next(0, 3)];
                SocketUser self = _client.CurrentUser;
                if (jan == arr[1])//あいこで
                {
                    await message.Channel.SendMessageAsync(message.Author.Mention + "→" + arr[1] + "　　" + self.Mention + "→" + jan + "\r\n" + "あいこです。");
                }
                if (arr[1] == "グー" && jan == "パー")//負け
                {
                    await message.Channel.SendMessageAsync(message.Author.Mention + "→" + arr[1] + "　　" + self.Mention + "→" + jan + "\r\n" + "負けです。");
                }
                if (arr[1] == "チョキ" && jan == "グー")//負け
                {
                    await message.Channel.SendMessageAsync(message.Author.Mention + "→" + arr[1] + "　　" + self.Mention + "→" + jan + "\r\n" + "負けです。");
                }
                if (arr[1] == "パー" && jan == "チョキ")//負け
                {
                    await message.Channel.SendMessageAsync(message.Author.Mention + "→" + arr[1] + "　　" + self.Mention + "→" + jan + "\r\n" + "負けです。");
                }
                if (arr[1] == "グー" && jan == "チョキ")//勝ち
                {
                    await message.Channel.SendMessageAsync(message.Author.Mention + "→" + arr[1] + "　　" + self.Mention + "→" + jan + "\r\n" + "勝ちです。");
                }
                if (arr[1] == "チョキ" && jan == "パー")//勝ち
                {
                    await message.Channel.SendMessageAsync(message.Author.Mention + "→" + arr[1] + "　　" + self.Mention + "→" + jan + "\r\n" + "勝ちです。");
                }
                if (arr[1] == "パー" && jan == "グー")//勝ち
                {
                    await message.Channel.SendMessageAsync(message.Author.Mention + "→" + arr[1] + "　　" + self.Mention + "→" + jan + "\r\n" + "勝ちです。");
                }
            }
            if (message.ToString() == "m?設定リスト")
            {
                await message.Channel.SendMessageAsync("A:コマンドの無効\r\n  サイコロ:1・ブロック:2・おみくじ:3・時間:4\r\n  gg:5・ランダム:6・カウンター:7・VCM:8・じゃんけん:9");
            }
            if (message.ToString().StartsWith("m?設定"))//この設定はサーバー共通です
            {
                if (message.ToString() == "m?設定リスト" || message.ToString() == "m?設定エクスポート" || message.ToString().StartsWith("m?設定インポート"))
                {

                }
                else
                {
                    string[] del = { "m?設定 " };
                    string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                    string[] arra = arr[1].Split(',');
                    if (arra[0] == "A")
                    {
                        byte byt = byte.Parse(arra[1]);
                        byt = (byte)(byt - 1);
                        if (bol[byt])
                        {
                            bol[byt] = false;
                        }
                        else
                        {
                            bol[byt] = true;
                        }
                        await message.Channel.SendMessageAsync("A," + (byt + 1).ToString() + "番 のコマンドを" + bol[byt].ToString() + "にしました。");
                    }
                    if (arra[0] == "RR" && arra[1] != "確認")
                    {
                        int it = int.Parse(arra[2]);
                        ulong ulg = ulong.Parse(arra[1]);
                        ulgkakko[it] = ulg;
                        await message.Channel.SendMessageAsync("Bの設定リスト<uei[" + it.ToString() + "]>を" + ulgkakko[it].ToString() + "にしました。");
                    }
                    if (arra[0] == "RR" && arra[1] == "確認")
                    {
                        string str = string.Join(' ', ulgkakko);
                        await message.Channel.SendMessageAsync(str);
                    }
                }
            }
            if (message.ToString() == "m?設定エクスポート")
            {
                string exp1 = string.Join(",", bol);
                await message.Channel.SendMessageAsync(exp1);
            }
            if (message.ToString().StartsWith("m?設定インポート"))
            {
                string[] del = { "m?設定インポート " };
                string[] arr = message.ToString().Split(del, StringSplitOptions.None);
                string[] arra = arr[1].Split(',');
                for (int i = 0; i < 9; i++)
                {
                    bol[i] = bool.Parse(arra[i]);
                }
                await message.Channel.SendMessageAsync("インポートしました。");
            }
            /*if (message.ToString().StartsWith("m?タイマー"))//実力不足
            {
                string[] arr = message.ToString().Split(' ', 'h', 'm', 's');
                string[] arr2 = DateTime.Now.ToString().Split('/', ':');
                double[] db = { 0, 0, 0 };
                for (int aa = 0; aa < 4; aa++)
                {
                    db[aa] = double.Parse(arr[aa + 1]);
                }
                double ddb = db[0] * 3600 + db[1] * 60 + db[2];
                DateTime dt = DateTime.Now;
                dt = dt.AddSeconds(ddb);

                //arr : [0]m?タイマー [1]時間[2]分[3]秒
                //arr2: [0]年[1]月[2]日[3]時[4]分[5]秒

            }*/
            if (message.ToString() == "m?おやすみ")
            {
                await message.Channel.SendMessageAsync("お…や…す……( ˘ω˘ ) ｽﾔｧ");
            }
            /*if (message.ToString().StartsWith("m?サーバー管理"))//まだ追加しない
            {
                string[] hikisuu = message.ToString().Split(' ', ',');
                if (string.Join(null, hikisuu).Length <= 9)
                {
                    var embed = new EmbedBuilder() // Embedのビルダーを宣言……面倒なのでBuild()まで含めてしまいます。
                  .WithAuthor(message.Author.Username,  // ここでユーザー名とアイコンを取得
                   message.Author.GetAvatarUrl() ?? message.Author.GetDefaultAvatarUrl())
                  .WithTitle("m?サーバー管理") // タイトルを設定
                  .WithDescription("引数 : m?サーバー管理 [種類],[設定1],[設定2]\r\n[種類]:R=このチャンネルに権限ロールを追加\r\nP=ロールに権限を設定\r\nG=特定の人に特定のロール付与") // 説明を設定
                  .WithColor(Color.Teal) //サイドの色を設定
                  .AddField("種類 = Rのとき", "[設定1]:ロールの名前,[設定2]permission数値", true)
                  .AddField("種類 = Pのとき", "[設定1]:ロールの名前,[設定2]permission数値", true)
                  .AddField("種類 = Gのとき", "[設定1]:つける人のID,[設定2]:ロールの名前", true)
                  .WithCurrentTimestamp() // 実行日時を追加
                  .Build(); // ビルド
                    await message.Channel.SendMessageAsync(null, false, embed);
                }
                else
                {
                    if (hikisuu[1] == "R")
                    {
                        var role = scktgid.Roles.FirstOrDefault(x => x.Name == hikisuu[2]);
                        OverwritePermissions overwritePermissions = OverwritePermissions.DenyAll(scktgid.GetChannel(message.Channel.Id) as IChannel);
                        await scktgid.GetChannel(message.Channel.Id).AddPermissionOverwriteAsync(role, overwritePermissions);
                    }
                }
            }*/
        }
        public void writeline(string write)
        {
            Console.WriteLine(write);
            try
            {
                StreamWriter writer = new StreamWriter($"{Settng.Settngs.path}manmen4.log", true, Encoding.GetEncoding("UTF-8"));
                writer.WriteLine(write);
                //Thread.Sleep(10000);
                writer.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine("wiriteline - errer!!!!!!!!!!!!!!!!!!");
                Console.WriteLine(exc.Message);
            }
        }
        public bool IsDMmessage(SocketMessage message)
        {
            if (message.Channel.ToString().StartsWith('@'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
