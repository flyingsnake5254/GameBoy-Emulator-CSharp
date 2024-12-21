using Gtk;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

public class Program
{
    // 視窗
    private static Window window;

    // 鍵盤
    public static Keyboard keyboard;

    // BAR
    private static VBox vBox;
    private static MenuBar menuBar;

    // 檔案
    private static MenuItem fileMenuItem;
    private static Menu fileMenuItemSub;
    private static MenuItem openFileMenuItem;

    // 遊戲速度
    private static MenuItem speedMenuItem;
    private static Menu speedMenuItemSub;

    // 存檔
    private static MenuItem saveMenuItem;
    private static Menu saveMenuItemSub;


    // *存檔紀錄
    private static MenuItem saveRecordMenuItem1;
    private static MenuItem saveRecordMenuItem2;
    private static MenuItem saveRecordMenuItem3;
    private static Record[] records = new Record[3];

    // 載入
    private static MenuItem loadMenuItem;
    private static Menu loadMenuItemSub;


    // *載入紀錄
    private static MenuItem loadRecordMenuItem1;
    private static MenuItem loadRecordMenuItem2;
    private static MenuItem loadRecordMenuItem3;

    // 資料庫
    private static string DatabasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gameboy_emulator.db");
    private static string ConnectionString = $"Data Source={DatabasePath};";

    // 遊戲物件
    private static Emulator emulator;
    public static DrawingArea drawingArea;
    private static string GameName = "";

    private static void Main(string[] args)
    {
        
        InitializeDatabase();
        keyboard = new Keyboard(); // 鍵盤
        UI();
        
    }

    /*
        UI
    */
    private static void UI()
    {
        Application.Init();
        BuildWindow();
        Application.Run();
    }

    private static void BuildWindow()
    {
        // 視窗
        window = new Window("Gameboy 模擬器");
        window.SetDefaultSize(Global.SCREEN_WIDTH * Global.SCREEN_SCALE, Global.SCREEN_HEIGHT * Global.SCREEN_SCALE);
        window.DeleteEvent += (o, e) => Application.Quit();
        window.SetPosition(WindowPosition.Center);

        BuildBar();

        // 確保窗口本身可以接受焦點
        window.KeyPressEvent += (o, args) =>
        {
            keyboard.HandleKeyDown((Gdk.EventKey)args.Event);
        };

        window.KeyReleaseEvent += (o, args) =>
        {
            keyboard.HandleKeyUp((Gdk.EventKey)args.Event);
        };

        window.ShowAll();
    }

    private static void BuildBar()
    {
        // 垂直容器
        vBox = new VBox(false, 2);
        menuBar = new MenuBar();

        BarFile();
        BarGameSpeed();
        BarSave();
        BarLoad();

        vBox.PackStart(menuBar, false, false, 0);
        window.Add(vBox);
    }

    private static void BarLoad()
    {
        // **載入
        loadMenuItem = new MenuItem("載入");
        loadMenuItemSub = new Menu();
        loadMenuItem.Submenu = loadMenuItemSub;
        
        // *紀錄
        loadRecordMenuItem1 = new MenuItem("----/--/--");
        loadRecordMenuItem2 = new MenuItem("----/--/--");
        loadRecordMenuItem3 = new MenuItem("----/--/--");

        loadRecordMenuItem1.Activated += (sender, e) =>
        {
            if (vBox.Children.Length > 1) // 假設只有一個 MenuBar，其他是 DrawingArea
            {
                vBox.Remove(vBox.Children[1]); // 移除舊的遊戲畫布
            }
            GetRecords(GameName);
            // emulator = new Emulator();
            GameObjInit();
            emulator.LoadEmulator(records[0]);
        };

        loadRecordMenuItem2.Activated += (sender, e) =>
        {
            if (vBox.Children.Length > 1) // 假設只有一個 MenuBar，其他是 DrawingArea
            {
                vBox.Remove(vBox.Children[1]); // 移除舊的遊戲畫布
            }
            GetRecords(GameName);
            // emulator = new Emulator();
            GameObjInit();
            emulator.LoadEmulator(records[1]);
        };

        loadRecordMenuItem3.Activated += (sender, e) =>
        {
            if (vBox.Children.Length > 1) // 假設只有一個 MenuBar，其他是 DrawingArea
            {
                vBox.Remove(vBox.Children[1]); // 移除舊的遊戲畫布
            }
            GetRecords(GameName);
            // emulator = new Emulator();
            GameObjInit();
            emulator.LoadEmulator(records[2]);
        };

        loadMenuItemSub.Append(loadRecordMenuItem1);
        loadMenuItemSub.Append(loadRecordMenuItem2);
        loadMenuItemSub.Append(loadRecordMenuItem3);
        menuBar.Append(loadMenuItem);
    }

    private static void BarSave()
    {
        // **存檔
        saveMenuItem = new MenuItem("存檔");
        saveMenuItemSub = new Menu();
        saveMenuItem.Submenu = saveMenuItemSub;
        
        // *紀錄
        saveRecordMenuItem1 = new MenuItem("----/--/--");
        saveRecordMenuItem2 = new MenuItem("----/--/--");
        saveRecordMenuItem3 = new MenuItem("----/--/--");

        saveRecordMenuItem1.Activated += (sender, e) =>
        {
            if (GameName != "")
            {
                Console.WriteLine("record 0");
                Record temp = emulator.GetGameData();
                SaveRecordToDatabase(GameName, "record0", temp);
                GetRecords(GameName);
            }
        };

        saveRecordMenuItem2.Activated += (sender, e) =>
        {
            if (GameName != "")
            {
                Console.WriteLine("record 1");
                Record temp = emulator.GetGameData();
                SaveRecordToDatabase(GameName, "record1", temp);
                GetRecords(GameName);
            }
        };

        saveRecordMenuItem3.Activated += (sender, e) =>
        {
            if (GameName != "")
            {
                Console.WriteLine("record 2");
                Record temp = emulator.GetGameData();
                SaveRecordToDatabase(GameName, "record2", temp);
                GetRecords(GameName);
            }
        };

        saveMenuItemSub.Append(saveRecordMenuItem1);
        saveMenuItemSub.Append(saveRecordMenuItem2);
        saveMenuItemSub.Append(saveRecordMenuItem3);
        menuBar.Append(saveMenuItem);
    }

    private static void BarGameSpeed()
    {

        speedMenuItem = new MenuItem("遊戲速度");
        speedMenuItemSub = new Menu();
        speedMenuItem.Submenu = speedMenuItemSub;

        // *遊戲速度選項
        AddSpeedOption(speedMenuItemSub, "0.25", 0.25);
        AddSpeedOption(speedMenuItemSub, "0.5", 0.5);
        AddSpeedOption(speedMenuItemSub, "0.75", 0.75);
        AddSpeedOption(speedMenuItemSub, "1", 1.0, true); // 預設為 1
        AddSpeedOption(speedMenuItemSub, "1.25", 1.25);
        AddSpeedOption(speedMenuItemSub, "1.5", 1.5);
        AddSpeedOption(speedMenuItemSub, "1.75", 1.75);
        AddSpeedOption(speedMenuItemSub, "2", 2.0);

        menuBar.Append(speedMenuItem);
    }

    private static void BarFile()
    {
        fileMenuItem = new MenuItem("檔案");
        fileMenuItemSub = new Menu();
        fileMenuItem.Submenu = fileMenuItemSub;

        // 開啟檔案
        openFileMenuItem = new MenuItem("開啟檔案");
        openFileMenuItem.Activated += (sender, e) =>
        {
            FileChooserDialog fileChooser = new FileChooserDialog(
                "選擇 gb 檔案",
                window,
                FileChooserAction.Open,
                "取消", ResponseType.Cancel,
                "開啟", ResponseType.Accept
            );

            // 只顯示 .gb 檔案
            FileFilter fileFilter = new FileFilter();
            fileFilter.Name = "Gameboy ROM Files (*.gb)";
            fileFilter.AddPattern("*.gb");
            fileChooser.Filter = fileFilter;

            // 選擇完檔案
            if (fileChooser.Run() == (int) ResponseType.Accept)
            {
                if (vBox.Children.Length > 1) // 假設只有一個 MenuBar，其他是 DrawingArea
                {
                    vBox.Children[1].Destroy();
                    vBox.Remove(vBox.Children[1]); // 移除舊的遊戲畫布
                }
                GameName = fileChooser.Filename;
                GameObjInit();
                fileChooser.Destroy();
                EmulatorInit();
            }
            else
            {
                fileChooser.Destroy();
            }
            
        };
        fileMenuItemSub.Append(openFileMenuItem);

        menuBar.Append(fileMenuItem);
    }
    public static void EmulatorInit()
    {
        emulator = new Emulator();
        emulator.DefaultInit(GameName);
        GetRecords(GameName);
    }
    public static void GameObjInit()
    {
        drawingArea = new DrawingArea();
        drawingArea.SetSizeRequest(Global.SCREEN_WIDTH * Global.SCREEN_SCALE, Global.SCREEN_HEIGHT * Global.SCREEN_SCALE);
        drawingArea.CanFocus = true;
        drawingArea.FocusOnClick = true;
        drawingArea.GrabFocus();

        drawingArea.KeyPressEvent += (o, args) =>
        {
            keyboard.HandleKeyDown((Gdk.EventKey)args.Event);
        };

        drawingArea.KeyReleaseEvent += (o, args) =>
        {
            keyboard.HandleKeyUp((Gdk.EventKey)args.Event);
        };

        vBox.PackStart(drawingArea, true, true, 0);
        drawingArea.Show();
    }
    private static void AddSpeedOption(Menu speedMenu, string label, double value, bool isDefault = false)
    {
        MenuItem menuItem = new MenuItem(isDefault ? $"✔ {label}" : label);

        menuItem.Activated += (sender, e) =>
        {
            // 更新所有選單項的標籤
            foreach (MenuItem item in speedMenu)
            {
                item.Label = item.Label.Replace("✔ ", "");
            }

            // 為當前選項添加 "V "
            menuItem.Label = $"✔ {label}";

            // 更新遊戲速度
            Global.GAME_SPEED = value;

            // 在終端輸出當前選擇的速度
            Console.WriteLine($"遊戲速度已設置為 {value}x");
        };

        speedMenu.Append(menuItem);
    }

    /*
        資料庫
    */
    private static void InitializeDatabase()
    {
        // 如果資料庫檔案不存在，直接建立資料夾和檔案
        if (!File.Exists(DatabasePath))
        {
            Console.WriteLine("Database file does not exist. It will be created.");
        }

        // 確保資料夾存在
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(DatabasePath));

        // 建立資料表與索引
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            // 建立 Records 資料表
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    GameName TEXT NOT NULL,
                    RecordName TEXT NOT NULL,
                    Data TEXT NOT NULL,
                    UNIQUE(GameName, RecordName)
                );";
            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table 'Records' checked/created.");
            }

            // 建立索引
            string createIndexQuery = @"
                CREATE INDEX IF NOT EXISTS idx_game_record 
                ON Records (GameName, RecordName);";
            using (var command = new SqliteCommand(createIndexQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Index on 'GameName' and 'RecordName' created.");
            }
        }
    }

    private static void SaveRecordToDatabase(string gameName, string recordName, Record record)
    {
        string jsonData = JsonConvert.SerializeObject(record);
        SaveToDatabase(gameName, recordName, jsonData);
    }

    private static Record LoadRecordFromDatabase(string gameName, string recordName)
    {
        string jsonData = LoadFromDatabase(gameName, recordName);
        if (jsonData == null)
        {
            Console.WriteLine("Record not found.");
            return null;
        }
        Record temp = JsonConvert.DeserializeObject<Record>(jsonData);

        return temp;
    }

    private static void SaveToDatabase(string gameName, string recordName, string jsonData)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string query = @"
                INSERT INTO Records (GameName, RecordName, Data)
                VALUES (@GameName, @RecordName, @Data)
                ON CONFLICT(GameName, RecordName) DO UPDATE SET
                Data = @Data";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@GameName", gameName);
                command.Parameters.AddWithValue("@RecordName", recordName);
                command.Parameters.AddWithValue("@Data", jsonData);
                command.ExecuteNonQuery();
            }
        }
    }


    private static string LoadFromDatabase(string gameName, string recordName)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string query = @"
                SELECT Data 
                FROM Records 
                WHERE GameName = @GameName AND RecordName = @RecordName";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@GameName", gameName);
                command.Parameters.AddWithValue("@RecordName", recordName);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                }
            }
        }

        // 若查詢不到記錄，回傳 null
        return null;
    }


    private static void GetRecords(string gameName)
    {
        if (gameName != null && gameName != "")
        {
            records[0] = LoadRecordFromDatabase(gameName, "record0");
            records[1] = LoadRecordFromDatabase(gameName, "record1");
            records[2] = LoadRecordFromDatabase(gameName, "record2");
            
            saveRecordMenuItem1.Label = records[0] == null ? "尚無存檔" : records[0].Timestamp.ToString("yyyy/MM/dd HH:mm:ss");
            saveRecordMenuItem2.Label = records[1] == null ? "尚無存檔" : records[1].Timestamp.ToString("yyyy/MM/dd HH:mm:ss");
            saveRecordMenuItem3.Label = records[2] == null ? "尚無存檔" : records[2].Timestamp.ToString("yyyy/MM/dd HH:mm:ss");

            loadRecordMenuItem1.Label = records[0] == null ? "尚無存檔" : records[0].Timestamp.ToString("yyyy/MM/dd HH:mm:ss");
            loadRecordMenuItem2.Label = records[1] == null ? "尚無存檔" : records[1].Timestamp.ToString("yyyy/MM/dd HH:mm:ss");
            loadRecordMenuItem3.Label = records[2] == null ? "尚無存檔" : records[2].Timestamp.ToString("yyyy/MM/dd HH:mm:ss");

        }
    }

}