﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenApi.rubyreceive {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:WashOut", ConfigurationName="rubyreceive.receive_port")]
    public interface receive_port {
        
        [System.ServiceModel.OperationContractAttribute(Action="receive_opt10081", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        OpenApi.rubyreceive.receive_opt10081Response receive_opt10081(OpenApi.rubyreceive.receive_opt10081Request request);
        
        // CODEGEN: 작업에 여러 개의 반환 값이 있기 때문에 메시지 계약을 생성하는 중입니다.
        [System.ServiceModel.OperationContractAttribute(Action="receive_opt10081", ReplyAction="*")]
        System.Threading.Tasks.Task<OpenApi.rubyreceive.receive_opt10081Response> receive_opt10081Async(OpenApi.rubyreceive.receive_opt10081Request request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="receive_opt10081", WrapperNamespace="urn:WashOut", IsWrapped=true)]
    public partial class receive_opt10081Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string value;
        
        public receive_opt10081Request() {
        }
        
        public receive_opt10081Request(string value) {
            this.value = value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="receive_opt10081Response", WrapperNamespace="urn:WashOut", IsWrapped=true)]
    public partial class receive_opt10081Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string value;
        
        public receive_opt10081Response() {
        }
        
        public receive_opt10081Response(string value) {
            this.value = value;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface receive_portChannel : OpenApi.rubyreceive.receive_port, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class receive_portClient : System.ServiceModel.ClientBase<OpenApi.rubyreceive.receive_port>, OpenApi.rubyreceive.receive_port {
        
        public receive_portClient() {
        }
        
        public receive_portClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public receive_portClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public receive_portClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public receive_portClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        OpenApi.rubyreceive.receive_opt10081Response OpenApi.rubyreceive.receive_port.receive_opt10081(OpenApi.rubyreceive.receive_opt10081Request request) {
            return base.Channel.receive_opt10081(request);
        }
        
        public void receive_opt10081(ref string value) {
            OpenApi.rubyreceive.receive_opt10081Request inValue = new OpenApi.rubyreceive.receive_opt10081Request();
            inValue.value = value;
            OpenApi.rubyreceive.receive_opt10081Response retVal = ((OpenApi.rubyreceive.receive_port)(this)).receive_opt10081(inValue);
            value = retVal.value;
        }
        
        public System.Threading.Tasks.Task<OpenApi.rubyreceive.receive_opt10081Response> receive_opt10081Async(OpenApi.rubyreceive.receive_opt10081Request request) {
            return base.Channel.receive_opt10081Async(request);
        }
    }
}
