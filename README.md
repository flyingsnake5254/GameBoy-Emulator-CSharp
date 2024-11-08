### Part 1

# 完成功能
- 讀取、解析卡匣 Header

# 運行指令
```bash
dotnet run path/to/file.gb
```


# 運行結果
```bash
V 參數輸入正確
卡匣 ROM Size : 32768 bytes
Title                | DMG-ACID2     
CGB Flag             | 0                   
Manufacturer Code    |                     
Cartridge Type       | ROM ONLY            
ROM Size Value       | 0                   
ROM Size             | 32768 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 0                   
Header Check Sum     | 159                  PASSED
V 讀取卡匣成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
X CPU 尚未實現
X CPU 停止運行
```


### Part 2

# 完成功能
- 初步完成 CPU Instruction Set

# 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


# 運行結果
```bash
V 參數輸入正確
卡匣 ROM Size : 32768 bytes
Title                | DMG-ACID2     
CGB Flag             | 0                   
Manufacturer Code    |                     
Cartridge Type       | ROM ONLY            
ROM Size Value       | 0                   
ROM Size             | 32768 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 0                   
Header Check Sum     | 159                  PASSED
V 讀取卡匣成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
Execute() : Current Opcode - 00 | PC : 0101
Execute() : Current Opcode - C3 | PC : 0104
Unknown Instruction : CE
```