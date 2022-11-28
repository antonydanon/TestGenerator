using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestGeneratorLibrary.Model;
using TestGeneratorLibrary.Service.Generation;

namespace TestGeneratorLibrary.Service.Pipeline
{
    public class PipelineService
    {
        private readonly TestGeneratorService _testsGenerator = new (new (new ()));

        private TransformBlock<string, string> _readerBlock;
        private TransformManyBlock<string, FileInformation> _generatorBlock;
        private ActionBlock<FileInformation> _writerBlock;

        private readonly DataflowLinkOptions _linkOptions = new DataflowLinkOptions() { PropagateCompletion = true};

        private readonly CodeReader _codeReader = new();
        private readonly CodeWriter _codeWriter = new();
        
        public PipelineService(int readTaskRestriction, int generateTaskRestriction, 
            int writeTaskRestriction, string savePath)
        {
            InitReadBlock(readTaskRestriction);
            InitGeneratorBlock(generateTaskRestriction);
            InitWriteBlock(writeTaskRestriction, savePath);
            SetLinks();
        }
        
        private void InitReadBlock(int readTaskRestriction)
        {
            _readerBlock = new TransformBlock<string, string>(
                async fileName => await _codeReader.Read(fileName),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = readTaskRestriction }
                );    
        }
        
        private void InitGeneratorBlock(int generateTaskRestriction)
        {
            _generatorBlock =
                new TransformManyBlock<string, FileInformation>(source =>
                        _testsGenerator.Generate(source),
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = generateTaskRestriction }); 
        }
        
        private void InitWriteBlock(int writeTaskRestriction, string savePath)
        {
            _writerBlock = new ActionBlock<FileInformation>(async fileInfo =>
            {
                await _codeWriter.Write(savePath + "\\" + fileInfo.FileName + ".cs", fileInfo.FileContent);
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = writeTaskRestriction });
        }
        
        private void SetLinks()
        {
            _readerBlock.LinkTo(_generatorBlock, _linkOptions);
            _generatorBlock.LinkTo(_writerBlock, _linkOptions);
        }

        public async Task Generate(List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                _readerBlock.Post(fileName);
            }
        
            _readerBlock.Complete();
            await _writerBlock.Completion;
        }
    }
}