# Game-Tricks

![仓库大小](https://img.shields.io/github/repo-size/zhangbincheng1997/Game-Tricks.svg) ![Unity](https://img.shields.io/badge/made%20with-Unity-brightgreen.svg) ![Lua](https://img.shields.io/badge/made%20with-Lua-brightgreen.svg) ![love](https://img.shields.io/badge/built%20with-love-pink.svg)

- [客户端](#客户端)
  - [场景](#场景)：10 scenes...
  - [图形学](#图形学)：光照、透视
  - [算法](#算法)：寻路、状态机、管理类
  - [架构](#架构)：DOTS
- [服务端](#服务端)
  - [Lua](#Lua)：C++调用Lua、Lua调用C++、面向对象、垃圾回收、高性能Lua、字节码、其他。
  - [游戏库](#游戏库)：热更新、序列化、运算等。
  - [性能优化](#性能优化)：火焰图、Postman、Jmeter

## 客户端
### 场景
#### NO.1 在圆内随机生成点
圆内的笛卡尔方程：
![math](https://render.githubusercontent.com/render/math?math=x%5E2%20%2B%20y%5E2%20%3C%3D%20r%5E2)
x^2 + y^2 = r^2

![math](https://render.githubusercontent.com/render/math?math=x%5E2%20%2B%20y%5E2%20%3D%20random%20%2A%20r%5E2)
[^_^]: https://render.githubusercontent.com/render/math?math=x^2 %2b y^2 = random * r^2

极坐标：
![math](https://render.githubusercontent.com/render/math?math=%5Crho%20%3D%20%5Csqrt%7Brandom%7D%20%2A%20r)
[^_^]: # (https://render.githubusercontent.com/render/math?math=\rho = \sqrt{random} * r)

![math](https://render.githubusercontent.com/render/math?math=%5Ctheta%20%3D%202%20%2A%20%5Cpi%20%2A%20random)
[^_^]: # (https://render.githubusercontent.com/render/math?math=\theta = 2 * \pi * random)

随机点(x, y)：
![math](https://render.githubusercontent.com/render/math?math=x%20%3D%20x%5C_center%20%2B%20%5Crho%20%2A%20cos%28%5Ctheta%29)
[^_^]: # (https://render.githubusercontent.com/render/math?math=x = x = x\_center %2b \rho * cos(\theta))
![math](https://render.githubusercontent.com/render/math?math=y%20%3D%20y%5C_center%20%2B%20%5Crho%20%2A%20sin%28%5Ctheta%29)
[^_^]: # (https://render.githubusercontent.com/render/math?math=y = y\_center %2b \rho * sin(\theta))

#### NO.2 范围检测
```
// 玩家位置
Vector3 pos = transform.position;
// 目标位置
Vector3 tarPos = target.position;
// 计算距离
float distance = Vector3.Distance(pos, tarPos);
// 玩家正方向
Vector3 normal = transform.rotation * Vector3.forward;
// 玩家到目标的方向
Vector3 offset = tarPos - pos;
// 计算夹角
float angle = Mathf.Acos(Vector3.Dot(normal.normalized, offset.normalized)) * Mathf.Rad2Deg;
```

#### NO.3 跑马灯
1. 原理
> * 跑马灯有区域限制，超出这个区域就不显示，这里我们用`Mask遮罩`实现。
> * 以水平跑马灯为例：跑马灯的可视范围是背景宽度，文字从右边开始到左边结束，总共移动的距离是`背景宽度 + 文字宽度`。
> * 跑马灯的动画实现使用了[`DOTween插件`](https://assetstore.unity.com/)。

2. 前期准备
> * 新建一个Image作为背景。调整适当大小。
> * 背景下再新建一个Image。添加Mask组件，用于遮住背景之外的文字，Rect Transfrom设置为Stretch，四维全部设置为0，铺满背景。
> 如果是水平滚动的将Rect Transform的Pivot设置为`1 0.5`，令Mask锚点位于`右边`。
> 如果是垂直滚动的将Rect Transform的Pivot设置为`0.5 0`，令Mask锚点位于`下边`。
> * Mask下创建Text，随意写些文字，居中显示，添加Content Size Fitter。
> 如果是水平滚动的将`Horizontal Fit`设置为Preferred Size，将Rect Transform的Pivot设置为`0 0.5`，令Text锚点位于Mask处，方便实现从右往左动画。
> 如果是垂直滚动的将`Vertical Fit`设置为Preferred Size，将Rect Transform的Pivot设置为`0.5 1`，令Text锚点位于Mask处，方便实现从下往上动画。

#### NO.4 插值移动
> * 创建2D精灵Sprite，命名为`Player`，作为我们的主角。
> * 切图。Sprite - Multiple - Sprite Editor（可能需要安装插件Package Manager - 2D Sprite） - Splice - Apply。
> * 制作人物休息和运动动画。按住CTRL选中几张帧动画图片拖到Inspector上的主角，可以快速生成动画，命名为`Idle`和`Run`，同时生成主角同名动画状态机Player。
> * 双击Player动画状态机可以直接打开Animator视图，将Idle和Run拖到视图，分别右键`Make Transition`。
> * 在Animator视图的左侧可以选择Parameters，创建Bool型参数`Run`，作为我们的转换条件。
> * 通过上面的步骤，我们设置PlayerIdle到PlayerRun的转换条件为Run `True`，PlayerRun到PlayerIdle的转化条件为Run `False`。
> * `Has Exit Time` = False
> * `Transtion Duration` = 0
> * 否则动画切换的时候会不及时，因为转换到下一个动画之前必须等待当前动画播放完毕。

> * 线性插值 `Vector3.Lerp(Vector3 from, Vector3 to, float smoothing)` 。
> * 公式 `t = from + (to - from) * smoothing`。
> * from为初始位置，to为结束位置，smoothing为平滑速度，返回t为线性插值计算出来的向量，范围在 [0...1]之间。

#### NO.5 虚拟摇杆
1. `定义委托`
public delegate void JoyStickTouchBegin(Vector2 vec);  // 定义触摸开始事件委托
public delegate void JoyStickTouchMove(Vector2 vec);  // 定义触摸过程事件委托
public delegate void JoyStickTouchEnd();  // 定义触摸结束事件委托
2. `注册事件`
public event JoyStickTouchBegin OnJoyStickTouchBegin;  // 注册触摸开始事件
public event JoyStickTouchMove OnJoyStickTouchMove;  // 注册触摸过程事件
public event JoyStickTouchEnd OnJoyStickTouchEnd;  // 注册触摸结束事件
3. `使用接口`：PointerDownHandler, IPointerUpHandler, IDragHandler  
public void OnPointerDown(PointerEventData eventData)  // 触摸开始  
public void OnPointerUp(PointerEventData eventData)  // 触摸结束  
public void OnDrag(PointerEventData eventData)  // 触摸过程  
4. `返回摇杆的偏移量`  
```
private Vector2 GetJoyStickAxis(PointerEventData eventData)
{
    // 获取手指位置的世界坐标
    Vector3 worldPosition;
    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(selfTransform,
             eventData.position, eventData.pressEventCamera, out worldPosition))
        selfTransform.position = worldPosition;
    // 获取摇杆偏移量
    Vector2 touchAxis = selfTransform.anchoredPosition - originPosition;
    // 摇杆偏移量限制
    if (touchAxis.magnitude >= JoyStickRadius)
    {
        touchAxis = touchAxis.normalized * JoyStickRadius;
        selfTransform.anchoredPosition = touchAxis;
    }
    return touchAxis;
}
```

#### NO.6 小地图
> * `UI准备`：Mask圆形遮罩，Minimap小地图边框。
> * 添加一个新的相机，并命名为`Mini Camera`。然后将该相机设为 Player 的子对象，position设为(0, 10,0)，rotation设为(90, 0, 0)。
> * 渲染到UI层需要用到Render Texture来实现。依次点击菜单项Assets -> Create -> Render Texture新建Render Texture，并命名为`Minimap Render`。选中Mini Camera后将Target Texture设为Minimap Render。
> * 下面新建Canvas来添加UI元素。新建Raw Image，命名为`Map`，将Texture设为Minimap Render。
> * 下面新建Image，命名为`Mask`，为其添加Mask组件，并将Image的Source Image设为上面的圆形遮罩。最后将Map设为Mask的子对象。
> * 下面新建Image，命名为`Outline`，将Image的Source Image设为上面的小地图边框。
> * 为了让整个小地图移动起来更方便，新建一个空的GameObject命名为`Minimap`，并将所有对象设为Minimap子对象。
> * 最后层级如下：  
> `Minimap`  
> ---- `Mask`  
> -------- `Map`  
> ---- `Outline`  

#### NO.7 分页
> * 制作Grid  
> 1.新建Image，改名`Grid`作为头像。  
> 2.新建Image作为`Grid`子物体，改名为`Item`作为物品名字背景。  
> 3.新建Text作为`Item`子物体，改名为`Name`作为物品名字。  
> 4.将物体制作成Prefab，最后层次关系应该是：
> Grid  
> ----Item  
> --------Name  
> * 自动排版  
> 1.新建Panel，将Grid作为Panel子物体，再将Grid复制12份。  
> 2.在Panel下添加`Grid Layout Group`组件，调整Padding、Cell Size、Spacing到合适位置，可以看到子物体全部自动排版。

#### NO.8 聊天框
> * 重点难点：  
> 1.需要控制别人和自己聊天框Item的位置  
> 2.需要控制聊天框ScrollView的滚动  
> 3.需要控制聊天框Item的宽度高度  
> 4.需要控制聊天框ScrollView的伸长  
> 5.需要移除历史聊天框Item  
> * 基本UI组件有玩家输入框、发送按钮、聊天框Item、聊天框ScrollView。
> * 聊天框Item有left和right两种，分别是别人和自己，以自己的聊天框right为例子:  
> 1.新建一个Image作为`背景`，设置Anchor为(right, top)、Pivot为(1, 1)。  
> 2.在背景下新建一个Image作为`头像`，设置Anchor为(right, bottom)和一个Text作为`文字`。  
> 3.在头像下新建一个Text作为`名字`，设置Anchor为(right, middle)。  
> 4.挂上ChatUI脚本，专门控制UI显示。
> 5.将其制作成为Prefab，聊天框left同理。  
> * 聊天框ScrollView：  
> 新建一个ScrollView，设置Anchor为(stretch, stretch)，调整为适当大小。  

### 图形学
#### Light 光照
1. Lambert（兰伯特）：漫反射


![math](https://render.githubusercontent.com/render/math?math=I_%7Bdiff%7D%20%3D%20K_d%20%5Cast%20I_l%20%5Cast%20%28N%20%5Ccdot%20L%29)


![](https://render.githubusercontent.com/render/math?math=I_{diff} = K_d \ast I_l \ast (N \cdot L))

2. Half-Lambert（半-兰伯特）：漫反射优化


![math](https://render.githubusercontent.com/render/math?math=I_%7Bdiff%7D%20%3D%20K_d%20%5Cast%20I_l%20%5Cast%20%28%5Calpha%20%28N%20%5Ccdot%20L%29%20%2B%20%5Cbeta%29)


![](https://render.githubusercontent.com/render/math?math=I_{diff} = K_d \ast I_l \ast (\alpha (N \cdot L) %2b \beta))

3. Phong（冯氏）：高光反射


![math](https://render.githubusercontent.com/render/math?math=I_%7Bspec%7D%20%3D%20K_s%20%5Cast%20I_l%20%5Cast%20%28V%20%5Ccdot%20R%29%5E%7Bn_s%7D)


![](https://render.githubusercontent.com/render/math?math=I_{spec} = K_s \ast I_l \ast (V \cdot R)^{n_s})



![math](https://render.githubusercontent.com/render/math?math=R%20%3D%202%20%5Cast%20%28N%20%5Ccdot%20L%29%20%5Cast%20N%20-%20L)


![](https://render.githubusercontent.com/render/math?math=R = 2 \ast (N \cdot L) \ast N - L)

4. Blinn-Phong（布林-冯氏）：高光反射优化


![math](https://render.githubusercontent.com/render/math?math=I_%7Bspec%7D%20%3D%20K_s%20%5Cast%20I_l%20%5Cast%20%28N%20%5Ccdot%20H%29%5E%7Bn_s%7D)


![](https://render.githubusercontent.com/render/math?math=I_{spec} = K_s \ast I_l \ast (N \cdot H)^{n_s})



![math](https://render.githubusercontent.com/render/math?math=H%20%3D%20%5Cfrac%7BL%20%2B%20V%7D%7B%5Cvert%20L%20%2B%20V%20%5Cvert%7D)


![](https://render.githubusercontent.com/render/math?math=H = \frac{L %2b V}{\vert L %2b V \vert})

#### Xray 透视
第一遍透视绘制：ZWrite Off、Greater。（关闭深度缓存）

第二遍正常绘制：ZWrite On、LEqual。

### 算法
#### AStar 寻路算法
- 估价函数：f(n) = g(n) + h(n)
- g(n)：从起点到节点n的最短路径。
- h(n)：从节点n到终点的最短路径的启发值。
- 曼哈顿距离：h(n) = x + y
- 特殊情况：当h(n)等于0时，A*算法等于Dijkstra算法。
```
while(OPEN!=NULL)
{
    从OPEN表中取f(n)最小的节点n;
    if(n节点==目标节点)
        break;
    for(当前节点n的每个子节点X)
    {
        计算f(X);
        if(XinOPEN)
            if(新的f(X)<OPEN中的f(X))
            {
                把n设置为X的父亲;
                更新OPEN表中的f(n);
            }
        if(XinCLOSE)
            continue;
        if(Xnotinboth)
        {
            把n设置为X的父亲;
            求f(X);
            并将X插入OPEN表中;//还没有排序
        }
    }//endfor
    将n节点插入CLOSE表中;
    按照f(n)将OPEN表中的节点排序;//实际上是比较OPEN表内节点f的大小，从最小路径的节点向下进行。
}//endwhile(OPEN!=NULL)
```

#### FSM 状态机
- public enum Transition // 状态转换条件
```
NullTransition = 0
FindPlayer = 1
LosePlayer = 2
```

- public enum StateId // 状态唯一标识
```
NullStateId = 0
Patrol = 1
Chase = 2
```

- public abstract class FSMState // State基类
```
private Dictionary<Transition, StateId> transDict; // Transition字典
public StateId stateId; // State Id
public FSMSystem fsm; // 状态机
...
public void AddTransition(Transition trans, StateId id); // 添加Transition
public void RemoveTransition(Transition trans); // 移除Transition
public StateId GetState(Transition trans); // 获取State
public virtual void DoBeforeEnter() { } // 进入State之前
public virtual void DoBeforeExit() { } // 退出State之前
public abstract void DoUpdate(); // 在State中
```

- public class FSMSystem // 状态机系统
```
private Dictionary<StateId, FSMState> stateDict; // State字典
public FSMState currentState; // 当前State
...
public void AddState(FSMState state); // 添加State
public void RemoveState(FSMState state); // 移除State
public void DoTransition(Transition trans); // 执行Transition
public void StartState(StateId id); // 开始State
```

- public class NPCController // NPC控制器...
- public class ChaseState // 追逐状态...
- public class PatrolState // 巡逻状态...
- public class PlayerMove // 角色移动...
- public class FollowPlayer // 跟随角色...

#### Manager 管理类(Audio、Pool、Scene...)
- Singleton（普通单例）
```
public abstract class Singleton<T>
    where T : new()
{
    private static T _instance;
    private static object _lock = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 上锁，防止重复实例化
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
```

- UnitySingleton（组件单例）
```
public class UnitySingleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 利用反射创建 Unity 物体
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    // 利用反射创建 Unity 组件
                    _instance = obj.AddComponent(typeof(T)) as T;
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
```

### 架构
- [DOTS](https://unity.com/cn/dots/packages)：Data-Oriented Tech Stack，面向数据的技术堆栈
1. ECS：数据和逻辑解耦，CPU缓存友好。
2. Job System：多核编程。
3. Burst Compiler：优化编译。

- 导包：Window -> Package Manager -> Add package from git URL -> com.unity.rendering.hybrid
- 调试：Window -> Analysis -> Entity Debugger

> [UnityECS学习日记](https://blog.csdn.net/qq_36382054/category_9596750.html)
> [EntityComponentSystemSamples](https://github.com/Unity-Technologies/EntityComponentSystemSamples)

## 服务端
### Lua
[自行编译lib](https://blog.csdn.net/wujie_03/article/details/72881389)
VS创建静态库项目，将解压出来的[lua-5.3.5](http://www.lua.org/ftp/)目录下的src文件中的头文件和源文件添加到项目中，点击生成解决方案。
在项目目录lua5.3/Debug可以看到.lib文件，将.lib文件拷贝到lua-5.3.5目录下备用。
VS创建空项目。
（1）在项目属性 > 配置属性 > C/C++ > 常规 > 附加包含目录添加lua源代码所在目录
（2）在项目属性 > 配置属性 >连接器 > 常规 > 附加库目录添加lua5.3.lib所在目录
（3）在项目属性 > 配置属性 >连接器 > 输入 > 附加库依赖项写入 lua5.3.lib;

#### [C++调用Lua](https://www.jb51.cc/lua/729696.html)

#### [Lua调用C++](https://www.jb51.cc/lua/729695.html)

#### 面向对象
[云风的个人空间 : Lua 中实现面向对象](https://blog.codingnow.com/cloud/HomePage)
A:方法名(参数) = A.方法名(A, 参数)
setmetatable(table, metatable)：对指定table设置元表(metatable)，如果元表(metatable)中存在__metatable键值，setmetatable会失败。
__index：当你通过键来访问table的时候，如果这个键没有值，那么Lua就会寻找该table的metatable（假定有metatable）中的__index键。
```
local _class={}
 
function class(super)
	local class_type={}
	class_type.ctor=false
	class_type.super=super
	class_type.new=function(...) 
			local obj={}
			do
				local create
				create = function(c,...)
					if c.super then
						create(c.super,...)
					end
					if c.ctor then
						c.ctor(obj,...)
					end
				end
 
				create(class_type,...)
			end
			setmetatable(obj,{ __index=_class[class_type] })
			return obj
		end
	local vtbl={}
	_class[class_type]=vtbl
 
	setmetatable(class_type,{__newindex=
		function(t,k,v)
			vtbl[k]=v
		end
	})
 
	if super then
		setmetatable(vtbl,{__index=
			function(t,k)
				local ret=_class[super][k]
				vtbl[k]=ret
				return ret
			end
		})
	end
 
	return class_type
end
```
```
base_type=class()		-- 定义一个基类 base_type
 
function base_type:ctor(x)	-- 定义 base_type 的构造函数
	print("base_type ctor")
	self.x=x
end
 
function base_type:print_x()	-- 定义一个成员函数 base_type:print_x
	print(self.x)
end
 
function base_type:hello()	-- 定义另一个成员函数 base_type:hello
	print("hello base_type")
end
```
```
test=class(base_type)	-- 定义一个类 test 继承于 base_type
 
function test:ctor()	-- 定义 test 的构造函数
	print("test ctor")
end
 
function test:hello()	-- 重载 base_type:hello 为 test:hello
	print("hello test")
end
```
```
a=test.new(1)	-- 输出两行，base_type ctor 和 test ctor 。这个对象被正确的构造了。
a:print_x()	-- 输出 1 ，这个是基类 base_type 中的成员函数。
a:hello()	-- 输出 hello test ，这个函数被重载了。
```

#### 垃圾回收
案例一：
```
function A()
    collectgarbage("collect")--进行垃圾回收，减少干扰
    PrintCount()
    local a = {}
    for i = 1, 5000 do
        table.insert(a, {})
    end
    PrintCount()
    collectgarbage("collect")
    PrintCount()
end

A()
PrintCount()
collectgarbage("collect")
PrintCount()
```
第二次输出，可以得出分配内存为303-19=284kb。第三次输出，因为局部变量a还在生命周期内，所以手动回收内存并没有影响。第四次输出，因为Lua的自动回收是每隔一段时间进行的，所以无影响。第五次输出，在执行手动回收后，分配的内存得到了回收，没有发生内存泄漏。

案例二：
```
function A()
    collectgarbage("collect")--进行垃圾回收，减少干扰
    PrintCount()
    a = {}--修改1
    for i = 1, 5000 do
        table.insert(a, {})
    end
    PrintCount()
    collectgarbage("collect")
    PrintCount()
end

A()
PrintCount()
collectgarbage("collect")
PrintCount()

--修改2
a = nil
collectgarbage("collect")
PrintCount()
```
第五次输出，因为a改为了全局变量，所以没办法进行回收。如果之后再也不需要使用a，那么就出现了内存泄漏了。为了避免这种情况，可以将a置空，此时a就会被lua判定为垃圾，就能进行回收了。因此，可以得出一个减少内存泄漏的方法：尽量用局部变量，这样当其生命周期结束时，就能被回收；对于全局变量，可以根据使用情况置空，及时回收内存。另外，如果某些情况出现或即将出现内存占用过大的情况，可以考虑手动去进行垃圾回收。

#### 高性能Lua
1. 前言：不要优化，还是不要优化。优化前后要做性能测试。
2. 基本事实：使用局部变量，避免动态编译。
3. 关于表：数组or哈希表，开放定址法。哈希的大小必须为2的幂。
老版Lua扩容会预分配空位，新版Lua扩容不会预分配空位，避免浪费内存空间，例子：点Point{x, y}。可以通过构造器避免重新哈希。
{true, true, true} => 3个空位
{x = 1, y = 2, z = 3} => 4个空位（浪费内存，浪费时间）
删除表元素，表不会缩小，更好的做法是删除表本身。
4. 关于字符串：Lua的字符串只有一份拷贝，变量只是引用类型。
5. 3R原则：减少reduce，重用reuse，回收recycle。
6. Tips：(1)LuaJIT；(2)Lua+C/C++。

#### 字节码
```
--luac -o test.luac test.lua
a = 18
print("hello world")
```
```
--python test.py
1b 4c 75 61 53 00 19 93 0d 0a 1a 0a 04 08 04 08
08 78 56 00 00 00 00 00 00 00 00 00 00 00 28 77
40 01 0a 40 74 65 73 74 2e 6c 75 61 00 00 00 00
00 00 00 00 00 01 02 05 00 00 00 08 40 40 80 06
80 40 00 41 c0 00 00 24 40 00 01 26 00 80 00 04
00 00 00 04 02 61 13 12 00 00 00 00 00 00 00 04
06 70 72 69 6e 74 04 0c 68 65 6c 6c 6f 20 77 6f
72 6c 64 01 00 00 00 01 00 00 00 00 00 05 00 00
00 02 00 00 00 03 00 00 00 03 00 00 00 03 00 00
00 03 00 00 00 00 00 00 00 01 00 00 00 05 5f 45
4e 56

.LuaS...........
.xV...........(w
@..@test.lua....
............@@..
.@.A...$@..&....
.....a..........
.print..hello wo
rld.............
................
.............._E
NV

signature: b'\x1bLua'
version: 53
format: 0
data: b'\x19\x93\r\n\x1a\n'
size_int: 4
size_size_t: 8
size_Instruction: 4
size_lua_Integer: 8
size_lua_Number: 8
Int: 5678
Num: 4077280000000000
sizeupvalues: 1
String值: b'@test.lua'
...
详细分析可见：Lua5.3.5的字节码
https://github.com/lua/lua/blob/063d4e4543088e7a21965bda8ee5a0f952a9029e/ldump.c
https://www.jianshu.com/p/f5ae9b7b235c
```

#### 其他
1. pairs和ipairs区别。
```
pairs: 迭代表。
t = { a = "apple", b = "baby", c = "cool" }
for k, v in pairs(t) do print(k, v) end

ipairs: 迭代数组。遇到nil退出。
a = {"one", "two", "three"}
for i, v in ipairs(t) do print(i, v) end
```
2. 获取长度
```
MT_KEY = {}
MT_KEY.__metatable = "READ_ONLY"

function getlen(t)
    if t[MT_KEY] ~= nil then
        return #(t[MT_KEY])
    else
        return #t
    end
end
```

### 游戏库
[lume](https://github.com/rxi/lume)
#### 热更新
```
暴力热更
function reload_module(module_name)
    package.loaded[module_name] = nil
    require(module_name)
end

优化热更
function reload_module(module_name)
    local old_module = _G[module_name]

    package.loaded[module_name] = nil
    require(module_name)

    local new_module = _G[module_name]
    for k, v in pairs(new_module) do
        old_module[k] = v
    end

    package.loaded[module_name] = old_module
end

Lume实现了类似的优化方案
lume.hotswap("lume") -- Reloads the lume module
assert(lume.hotswap("inexistant_module")) -- Raises an error
```
#### 序列化
```
自己实现
function serialize(t)
	local mark={}
	local assign={}
 
	local function ser_table(tbl,parent)
		mark[tbl]=parent
		local tmp={}
		for k,v in pairs(tbl) do
			local key= type(k)=="number" and "["..k.."]" or k
			if type(v)=="table" then
				local dotkey= parent..(type(k)=="number" and key or "."..key)
				if mark[v] then
					table.insert(assign,dotkey.."="..mark[v])
				else
					table.insert(tmp, key.."="..ser_table(v,dotkey))
				end
			else
				table.insert(tmp, key.."="..v)
			end
		end
		return "{"..table.concat(tmp,",").."}"
	end
 
	return ser_table(t,"ret")
end
 
t = { a = 1, b = 2, rt = {c = 3, d = 4} }
print(serialize(t)) -- {a=1,rt={c=3,d=4},b=2}

Lume实现
lume.serialize({a = "test", b = {1, 2, 3}, false}) -- Returns "{[1]=false,["a"]="test",["b"]={[1]=1,[2]=2,[3]=3,},}"
```
#### 运算
```
lume.randomchoice(t)
lume.weightedchoice(t)

lume.randomchoice({true, false}) -- Returns either true or false
lume.weightedchoice({ ["cat"] = 10, ["dog"] = 5, ["frog"] = 0 }) -- Returns either "cat" or "dog" with "cat" being twice as likely to be chosen.
```

### 性能优化
#### 火焰图
![](docs/火焰图.jpg)

火焰图是基于 perf 结果产生的 SVG 图片，用来展示 CPU 的调用栈。
y 轴表示调用栈，每一层都是一个函数。调用栈越深，火焰就越高，顶部就是正在执行的函数，下方都是它的父函数。
x 轴表示抽样数，如果一个函数在 x 轴占据的宽度越宽，就表示它被抽到的次数多，即执行的时间长。注意，x 轴不代表时间，而是所有的调用栈合并后，按字母顺序排列的。
火焰图就是看顶层的哪个函数占据的宽度最大。只要有"平顶"（plateaus），就表示该函数可能存在性能问题。
颜色没有特殊含义，因为火焰图表示的是 CPU 的繁忙程度，所以一般选择暖色调。

```
# 安装perf
yum install perf
# 下载火焰图工具
git clone https://github.com/brendangregg/FlameGraph.git
# 捕获堆栈
perf record -F 99 -p 181 -g -- sleep 60
【perf record表示记录，-F 99表示每秒99次，-p 13204表示分析的进程号，-g表示记录调用栈，sleep 30表示持续30秒。】
# 解析堆栈
perf script > out.stacks
# 折叠堆栈
./stackcollapse-perf.pl out.stacks > out.folded
# 生成svg火焰图
./flamegraph.pl out.folded > perf.svg
# 使用管道简化命令
perf script | ./stackcollapse-perf.pl | ./flamegraph.pl > perf.svg
# 红蓝差分火焰图（红色上升，蓝色下降。）
./difffolded.pl out.folded1 out.folded2 | ./flamegraph.pl > diff.svg

不足之处：http://www.brendangregg.com/blog/2014-11-09/differential-flame-graphs.html
虽然红/蓝差分火焰图很有用，但实际上还是有一个问题：如果一个代码执行路径完全消失了，那么在火焰图中就找不到地方来标注蓝色。你只能看到当前的CPU使用情况，而不知道为什么会变成这样。一个办法是，将对比顺序颠倒，画一个相反的差分火焰图。
./difffolded.pl out.folded1 out.folded2 | ./flamegraph.pl > diff2.svg
./difffolded.pl out.folded2 out.folded1 | ./flamegraph.pl --negate > diff1.svg
diff1.svg：宽度是以修改前profile文件为基准，颜色表明将要发生的情。
diff2.svg：宽度是以修改后profile文件为基准，颜色表明已经发生的情况。
```

案例分析
```
int i() { for(int i = 0; i < 20000; i++) {}; }

int e() { for(int i = 0; i < 20000; i++) {}; }

int g() { for(int i = 0; i < 50000; i++) {}; }

int h() { i(); }

int f() { g(); }

int d() { e(); f(); }

int c() { d(); }

int b() { c(); }

int a() { b(); h(); }
```

- 编译
g++ main1.cpp -o main1
- 运行
./main1
- 结果
![](docs/火焰图测试.png)

http://www.ruanyifeng.com/blog/2017/09/flame-graph.html
https://github.com/gatieme/LDD-LinuxDeviceDrivers/tree/master/study/debug/tools/perf/flame_graph
https://queue.acm.org/detail.cfm?id=2927301
https://zhuanlan.zhihu.com/p/73385693
https://zhuanlan.zhihu.com/p/73482910
https://www.jianshu.com/p/7ec8378f1a3c
https://www.jianshu.com/p/3cdc0f05ac5d
https://blog.openresty.com.cn/cn/dynamic-tracing/

#### [Postman](https://www.postman.com/)
调试工具，不要我多说了8。

#### [Jmeter](https://jmeter.apache.org/)
测压工具，不要我多说了8。




