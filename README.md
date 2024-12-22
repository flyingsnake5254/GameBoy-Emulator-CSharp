## Gameboy Emulator by C#
[PPT](https://docs.google.com/presentation/d/1yT8TKOju-b1cTyCFQHvN060rL6zyQQ3aPgBHsILw9PY/edit?usp=sharing)

<h6 style="color:red">註：作業系統須安裝 Gtk</h6>

#### 版本資訊
```sh
   [net9.0]: 
   Top-level Package            Requested    Resolved  
   > GtkSharp                   3.24.24.95   3.24.24.95
   > Microsoft.Data.Sqlite      9.0.0        9.0.0     
   > Newtonsoft.Json            13.0.3       13.0.3  
```
---

#### 專案建置與執行
```shell
# 取得專案
git clone https://github.com/flyingsnake5254/Gameboy-Emulator-CSharp.git

# 進入專案資料夾
cd Gameboy-Emulator-CSharp

# 運行專案
dotnet run
```
---

#### 專案執行檔位置
- Linux ( Manjaro )
    ```shell
    Release/linux-x64/publish/Gameboy-Emulator-CSharp
    ```

- Windows
    ```shell
    Release/win-x64/publish/Gameboy-Emulator-CSharp.exe
    ```

---

#### 匯出執行檔
- Linux (Manjaro)
    ```shell
    dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:EnableCompressionInSingleFile=true

    # 執行檔路徑
    # Gameboy-Emulator-CSharp/bin/Release/net9.0/linux-x64/publish

    # 檔名
    # Gameboy-Emulator-CSharp
    ```

- Windows
    ```shell
    dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:EnableCompressionInSingleFile=true

    # 執行檔路徑
    # Gameboy-Emulator-CSharp/bin/Release/net9.0/win-x64/publish

    # 檔名
    # Gameboy-Emulator-CSharp.exe
    ```

