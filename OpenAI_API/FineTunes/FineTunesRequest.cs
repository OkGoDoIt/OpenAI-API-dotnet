using Newtonsoft.Json;
using OpenAI_API.Models;
using System;

namespace OpenAI_API.FineTunes
{
    /// <summary>
    /// Manage fine-tuning jobs to tailor a model to your specific training data.
    /// </summary>
    public class FineTunesRequest
    {
        /// <summary>
        /// The ID of an uploaded file that contains training data.
        /// See upload file for how to upload a file.
        /// Your dataset must be formatted as a JSONL file, 
        /// where each training example is a JSON object with the keys "prompt" and "completion". 
        /// Additionally, you must upload your file with the purpose fine-tune.
        /// See the fine-tuning guide for more details.
        /// </summary>
        [JsonProperty("training_file", Required = Required.Always)]
        public string TrainingFile { get; set; }



        /// <summary>
        /// Optional
        /// The ID of an uploaded file that contains validation data.
        /// If you provide this file, the data is used to generate validation metrics periodically 
        /// during fine-tuning.These metrics can be viewed in the fine-tuning results file.
        /// Your train and validation data should be mutually exclusive.
        /// Your dataset must be formatted as a JSONL file, where each validation example 
        /// is a JSON object with the keys "prompt" and "completion". 
        /// Additionally, you must upload your file with the purpose fine-tune.
        /// See the fine-tuning guide for more details
        /// </summary>
        [JsonProperty("validation_file")]
        public string ValidationFile { get; set; }

        /// <summary>
        /// Optional
        /// Defaults to curie
        /// The name of the base model to fine-tune.
        /// You can select one of "ada", "babbage", "curie", "davinci", 
        /// or a fine-tuned model created after 2022-04-21. 
        /// To learn more about these models, see the Models documentation.
        /// </summary>
        [JsonProperty("model")]
        public Model Model { get; set; } = Model.CurieText;

        /// <summary>
        /// Optional
        /// Defaults to 4
        /// The number of epochs to train the model for. 
        /// An epoch refers to one full cycle through the training dataset.
        /// </summary>
        [JsonProperty("n_epochs")]
        public int NEpochs { get; set; } = 4;

        /// <summary>
        /// Optional
        /// Defaults to null
        /// The batch size to use for training.The batch size is the number of training examples used 
        /// to train a single forward and backward pass.
        /// By default, the batch size will be dynamically configured to be ~0.2% of the number 
        /// of examples in the training set, capped at 256 - in general, we've found that 
        /// larger batch sizes tend to work better for larger datasets.
        /// </summary>
        [JsonProperty("batch_size")]
        public int? BatchSize { get; set; } = null;

        /// <summary>
        /// Optional   
        /// Defaults to null
        /// The learning rate multiplier to use for training.The fine-tuning learning rate is the 
        /// original learning rate used for pretraining multiplied by this value.
        /// By default, the learning rate multiplier is the 0.05, 0.1, or 0.2 depending on final 
        /// batch_size (larger learning rates tend to perform better with larger batch sizes). 
        /// We recommend experimenting with values in the range 0.02 to 0.2 to see what produces 
        /// the best results.
        /// </summary>
        [JsonProperty("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; set; } = null;

        /// <summary>
        /// Optional
        /// Defaults to 0.01
        /// The weight to use for loss on the prompt tokens.This controls how much the model 
        /// tries to learn to generate the prompt(as compared to the completion which always 
        /// has a weight of 1.0), and can add a stabilizing effect to training when completions are short.
        /// If prompts are extremely long (relative to completions), it may make sense to reduce this weight so as to avoid over-prioritizing learning the prompt.
        /// </summary>
        [JsonProperty("prompt_loss_weight")]
        public double? PromptLossWeight { get; set; } = 0.01;

        /// <summary>
        /// Optional
        /// Defaults to false
        /// If set, we calculate classification-specific metrics such as accuracy and F-1 score using the validation set at the end of every epoch.These metrics can be viewed in the results file.
        /// In order to compute classification metrics, you must provide a validation_file.Additionally, you must specify classification_n_classes for multiclass classification or classification_positive_class for binary classification.
        /// </summary>
        [JsonProperty("compute_classification_metrics")]
        public bool ComputeClassificationMetrics { get; set; } = false;

        /// <summary>
        /// Optional
        /// Defaults to null
        /// The number of classes in a classification task.
        /// This parameter is required for multiclass classification.
        /// </summary>
        [JsonProperty("classification_n_classes")]
        public int? classification_n_classes { get; set; } = null;


        /// <summary>
        /// Optional
        /// Defaults to null
        /// The positive class in binary classification.
        /// This parameter is needed to generate precision, recall, and F1 metrics when doing binary classification.
        /// </summary>
        [JsonProperty("classification_positive_class")]
        public string classification_positive_class { get; set; } = null;


        /// <summary>
        /// Optional
        /// Defaults to null
        /// If this is provided, we calculate F-beta scores at the specified beta values.
        /// The F-beta score is a generalization of F-1 score.This is only used for binary classification.
        /// With a beta of 1 (i.e.the F-1 score), precision and recall are given the same weight. 
        /// A larger beta score puts more weight on recall and less on precision. A smaller beta score puts more weight on precision and less on recall.
        /// </summary>
        [JsonProperty("classification_betas")]
        public Array classification_betas { get; set; }


        /// <summary>
        /// Optional
        /// Defaults to null
        /// A string of up to 40 characters that will be added to your fine-tuned model name.
        /// For example, a suffix of "custom-model-name" 
        /// would produce a model name like ada:ft-your-org:custom-model-name-2022-02-15-04-21-04.
        /// </summary>
        [JsonProperty("suffix")]
        public string suffix { get; set; } = null;
    }
}
