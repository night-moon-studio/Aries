

namespace Microsoft.AspNetCore.Mvc
{

    public class AriesJsonResultController : ControllerBase
    {

        /// <summary>
        /// 分页 - 用布尔类型操作返回值
        /// </summary>
        /// <param name="value">true/false代表返回成功与否</param>
        /// <param name="totle">总条数</param>
        /// <param name="message">正确提示，默认：操作成功！错误提示，默认：操作失败！</param>
        /// <returns></returns>
        protected AriesJsonPageResult BoolResult(bool value, long totle, string message = null)
        {
            AriesJsonPageResult _result = new AriesJsonPageResult();
            _result.Totle = totle;
            if (value)
            {
                _result.Code = 0;
                _result.Msg = message == null ? "操作成功！" : message;
            }
            else
            {
                _result.Code = 1;
                _result.Msg = message == null ? "操作失败！" : message;
            }
            return _result;
        }
        /// <summary>
        /// 分页 - 返回对象，若对象为空，则返回错误信息
        /// </summary>
        /// <param name="value">需要传送的对象</param>
        /// <param name="totle"></param>
        /// <param name="message">正确提示，默认：操作成功！错误提示，默认：操作失败！</param>
        /// <returns></returns>
        protected AriesJsonPageResult Result(object value, long totle, string message = null)
        {
            AriesJsonPageResult _result = new AriesJsonPageResult();
            _result.Totle = totle;
            if (value != null)
            {
                _result.Data = value;
                _result.Code = 0;
                _result.Msg = message == null ? "操作成功！" : message;
            }
            else
            {
                _result.Code = 1;
                _result.Msg = message == null ? "操作失败！" : message;
            }
            return _result;
        }
        /// <summary>
        /// 用布尔类型操作返回值
        /// </summary>
        /// <param name="value">true/false代表返回成功与否</param>
        /// <param name="message">正确提示，默认：操作成功！错误提示，默认：操作失败！</param
        /// <returns></returns>
        protected AriesJsonResult BoolResult(bool value, string message = null)
        {
            AriesJsonResult _result = new AriesJsonResult();
            if (value)
            {
                _result.Code = 0;
                _result.Msg = message == null ? "操作成功！" : message;
            }
            else
            {
                _result.Code = 1;
                _result.Msg = message == null ? "操作失败！" : message;
            }
            return _result;
        }
        /// <summary>
        /// 返回对象，若对象为空，则返回错误信息
        /// </summary>
        /// <param name="value">需要传送的对象</param>
        /// <param name="message">正确提示，默认：操作成功！错误提示，默认：操作失败！</param
        /// <returns></returns>
        protected AriesJsonResult Result(object value, string message = null)
        {
            AriesJsonResult _result = new AriesJsonResult();
            if (value != null)
            {
                _result.Data = value;
                _result.Code = 0;
                _result.Msg = message == null ? "操作成功！" : message;
            }
            else
            {
                _result.Code = 1;
                _result.Msg = message == null ? "操作失败！" : message;
            }
            return _result;
        }



        /// <summary>
        /// 返回提示信息
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="status">状态码，默认1</param>
        /// <returns></returns>
        protected AriesJsonResult Error(string msg, int status = 1, object data = null)
        {
            AriesJsonResult _result = new AriesJsonResult();
            _result.Msg = msg;
            _result.Data = data;
            _result.Code = status;
            return _result;
        }
        /// <summary>
        /// 返回提示信息
        /// </summary>
        /// <param name="value">提示信息</param>
        /// <param name="status">状态码，默认1</param>
        /// <returns></returns>
        protected AriesJsonResult Error(int status, string msg, object data = null)
        {
            AriesJsonResult _result = new AriesJsonResult();
            _result.Msg = msg;
            _result.Data = data;
            _result.Code = status;
            return _result;
        }

    }
}
