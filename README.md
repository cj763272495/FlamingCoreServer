# 服务器设计

使用gPRC通信，玩家登录后返回令牌，后续调用都会验证令牌

## 1.架构设计

### 1.接入层

##### 		见接口定义

### 2.逻辑层

#### 	1.战斗模块

##### 		1.开始战斗 返回是否可以开始

##### 		2.结束战斗

#### 	2.玩家能量模块

##### 		1.玩家能量自动恢复

##### 		2.玩家能量消耗

##### 		3.玩家能量购买（即增加）

#### 	3.商店模块

##### 		1.购买核心

##### 		2.购买拖尾

### 3.数据存储层 

#### 	存储玩家数据到服务器Json文件，加载文件到内存，返回玩家数据

 



## 2.接口定义

### 1.登录注册模块

#### 	1.登录注册

​	`Login（string name, sting password）` 

​	登录和注册使用同一个接口，返回数据PlayerData类：

​	返回数据：

```
  "12": { 
    "coin": 5894,
    "skin": [
      0
    ],
    "trail": [
      0
    ],
    "energy": 22,
    "max_unLock_wave": 1,
    "cur_skin": 4,
    "cur_trail": 5
  }
```

### 2.战斗模块

#### 	1.开始关卡

​	`StartBattle（string token）`

​	返回：

​	`	{ canStart：bool }`

#### 	2.关卡结束

​		`EndBattle（string token,int level,int getCoins）`

​	返回:

​		`{sucess : bool }`

### 3.商店模块	

#### 1.购买物品

```
   enum GoodsType{
        Skin = 0;
        Trail = 1;
        Energy = 3;
    }
 	ShopPurchase(GoodsType type ,int id );
```

 返回：

​		`{sucess = bool}`