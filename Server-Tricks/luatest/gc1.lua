function PrintCount()
    print("内存为：", collectgarbage("count")) -- 输出当前内存占用
end

function A()
    collectgarbage("collect") -- 进行垃圾回收，减少干扰
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
