using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CSharpClassLibrary.StaticConstructor;

public class ViewModel<T>
{
    public ObservableCollection<T> Data { get; set; }

    //static async method that behave like a constructor       
    async public static Task<ViewModel<T>> BuildViewModelAsync()
    {
        //ObservableCollection<T> tmpData = await GetDataTask();
        ObservableCollection<T> tmpData = null;
        return new ViewModel<T>(tmpData);
    }

    // private constructor called by the async method
    private ViewModel(ObservableCollection<T> Data)
    {
        this.Data = Data;
    }
}
