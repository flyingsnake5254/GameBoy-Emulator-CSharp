# Part 1

### 完成功能
- 讀取、解析卡匣 Header

### 運行指令
```bash
dotnet run path/to/file.gb
```


### 運行結果
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

---

# Part 2

### 完成功能
- 初步完成 CPU 指令讀取

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


### 運行結果
```bash
參數輸入完成
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
卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
CPU 初始化成功
Bef : Current Opcode - 00 | PC : 0100
Aft : Current Opcode - 00 | PC : 0101
Execute 尚未實現
------------------
Bef : Current Opcode - 00 | PC : 0101
Aft : Current Opcode - C3 | PC : 0104
Execute 尚未實現
------------------
Bef : Current Opcode - C3 | PC : 0104
Unknown Instruction : CE
```

---

# Part 3

### 完成功能
- 初步完成 CPU 指令執行

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


### 運行結果
```bash
參數輸入完成
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
卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
CPU 初始化成功
PC: 0100 NOP (00, C3, 50) A:01 B:00 C:00
PC: 0101 JP (C3, 50, 01) A:01 B:00 C:00
PC: 0150 DI (F3, 31, FF) A:01 B:00 C:00
Unknown Instruction : 31
```