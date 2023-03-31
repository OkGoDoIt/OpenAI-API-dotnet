using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_API.FineTunes
{
    /// <summary>
    /// Manage fine-tuning jobs to tailor a model to your specific training data.
    /// <see cref=""/>
    /// </summary>
    public interface IFineTunesEndpoint
    {

        Task CreateFineTune();

        Task ListFineTunes();

        Task RetrieveFineTune();

        Task CancelFineTune();

        Task ListFineTuneEvents();
    }
}
