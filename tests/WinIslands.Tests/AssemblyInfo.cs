using Xunit;

// 这些测试通过全局环境变量 WINISLAND_APPDATA 重定向数据目录，
// 并行执行会互相干扰，因此禁用测试并行化。
[assembly: CollectionBehavior(DisableTestParallelization = true)]
