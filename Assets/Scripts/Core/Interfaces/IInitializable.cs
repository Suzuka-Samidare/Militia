using System.Threading.Tasks;

public interface IInitializable
{
    // 初期化処理本体
    Task Initialize();
}