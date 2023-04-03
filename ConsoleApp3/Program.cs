using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace ConsoleApp2
{
    class Program
    {


        public static readonly Dictionary<string, object> keys = new Dictionary<string, object>()
        {
            {"shop_site","站点名称" },
            { "shop_type","站点编码"},
            { "imgid","站点logo"},
            {"logo_sort","logo排序" },
            {"type","类型" },
            { "sort","站点排名"},
            { "status","开发进度"},
            { "version","接口版本"},
            {"enabled","站点状态" },
            { "is_other","其他"},
            { "co_ids","公司id"},
            { "syncOrder","订单下载"},
            { "syncItem","商品下载"},
            { "syncInventory","库存同步"},
            { "syncDelivery","同步发货"},
            { "serviceDownload","售后单下载"},
            { "syncInvoice","发票信息下载"},
            { "developer","开发人员"},
            { "developer1","产品负责人"},
            { "apicode","Api编号"},
            { "key_words","关键字"},
            { "tokenExpireIn","Token时长"},
            { "autoRefreshToken","自动刷新Token"},
            { "relationWay","站点联系方式"},
            { "authorizeExplain","授权说明"},
            { "remark","备注"}
        };


        static void Main(string[] args)
        {

            //最后结果的键值对
            Dictionary<string, object> retDic = new Dictionary<string, object>();
            //原始报文
            var originalJson = "{\"sync_order\":true,\"sync_item\":true,\"sync_inventory\":true,\"sync_delivery\":true,\"after_sale_service_download\":true,\"sync_invoice\":false,\"token_expire_in\":2592000,\"auto_refresh_token\":true,\"settings\":\"{\\\"id\\\":\\\"107\\\",\\\"hiddenEditModel\\\":\\\"update\\\",\\\"shop_site\\\":\\\"淘宝天猫\\\",\\\"shop_type\\\":\\\"Taobao\\\",\\\"contract_content\\\":\\\"\\\",\\\"imgid\\\":\\\"jst_contract/2021/03/temp/9073a891d5b645f9884a9f468dcb5fb4.jpg\\\",\\\"logo_sort\\\":\\\"1\\\",\\\"type\\\":\\\"Import\\\",\\\"sort\\\":\\\"1\\\",\\\"status\\\":\\\"Online\\\",\\\"onlineDate\\\":\\\"\\\",\\\"version\\\":\\\"1\\\",\\\"enabled\\\":\\\"true\\\",\\\"is_other\\\":\\\"true\\\",\\\"co_ids\\\":\\\"\\\",\\\"syncOrder\\\":true,\\\"syncItem\\\":true,\\\"syncInventory\\\":true,\\\"syncDelivery\\\":true,\\\"serviceDownload\\\":true,\\\"developer\\\":\\\"123\\\",\\\"developer1\\\":\\\"小虎\\\",\\\"apicode\\\":\\\"1\\\",\\\"key_words\\\":\\\"淘宝天猫\\\",\\\"tokenExpireIn\\\":\\\"30天\\\",\\\"autoRefreshToken\\\":\\\"true\\\",\\\"refreshTokenExpireIn\\\":86400,\\\"relationWay\\\":\\\"\\\",\\\"subscribe\\\":\\\"需要\\\",\\\"orderMoney\\\":\\\"5888\\\",\\\"authExplainUrl\\\":\\\"https://fuwu.taobao.com/ser/detail.htm?service_code=FW_GOODS-1000007023&tracelog=search&from_key=%E8%81%9A%E6%B0%B4%E6%BD%AD\\\",\\\"rebateMoney\\\":\\\"\\\",\\\"accredit\\\":\\\"账号登录式授权\\\",\\\"auth_explain_url\\\":\\\"https://www.erp321.com/app/support/document.html#page=34\\\",\\\"authorization_anual\\\":\\\"https://www.erp321.com/app/support/document.html#page=34\\\",\\\"switch_url\\\":\\\"\\\",\\\"nick_explain_url\\\":\\\"\\\",\\\"isAccurate\\\":\\\"\\\",\\\"api_url\\\":\\\"https://www.baidu.com\\\",\\\"permanentAuthorization\\\":\\\"否\\\",\\\"UseSameAuthOtherPlatform\\\":\\\"\\\",\\\"SupportWayBillShop\\\":\\\"支持\\\",\\\"DefaultAppCid\\\":\\\"0\\\",\\\"SessionUIdTextName\\\":\\\"\\\",\\\"BindWayBillValidOrder\\\":\\\"校验\\\",\\\"isStandardKeySecrcet\\\":\\\"否\\\",\\\"IsShowRefreshTokenExpired\\\":\\\"否\\\",\\\"AuthRequestUrl\\\":\\\"\\\",\\\"indentSync\\\":\\\"RDS推送\\\",\\\"order_download\\\":\\\"支持\\\",\\\"order_download1\\\":\\\"2\\\",\\\"longestTime\\\":\\\"123\\\",\\\"create_modify\\\":\\\"\\\",\\\"syncPhotoDownload\\\":\\\"支持\\\",\\\"photoSource\\\":\\\"线上\\\",\\\"order_i_id\\\":\\\"不支持\\\",\\\"order_sku_id\\\":\\\"支持\\\",\\\"color_specifications\\\":\\\"不支持\\\",\\\"resolution\\\":\\\"goodsNoNumber\\\",\\\"syncStatus\\\":\\\"待付款,已付款,已发货,关闭,取消\\\",\\\"order_apportion\\\":\\\"不支持\\\",\\\"costPrice\\\":\\\"不支持\\\",\\\"costPriceTitle\\\":\\\"\\\",\\\"indentMoney\\\":\\\"线上\\\",\\\"order_down_content\\\":\\\"\\\",\\\"sellerDemarkDowmload\\\":\\\"支持\\\",\\\"flag_upload\\\":\\\"不支持\\\",\\\"flag_down\\\":\\\"不支持\\\",\\\"sellerUploading\\\":\\\"支持\\\",\\\"sellerMessageDowmload\\\":\\\"支持\\\",\\\"onlineAdressDown\\\":\\\"支持\\\",\\\"erpAdressUploading\\\":\\\"支持\\\",\\\"onlineRefundStatus\\\":\\\"applyIntercept\\\",\\\"offlineCancelRefund\\\":\\\"支持\\\",\\\"express_delivery\\\":\\\"支持\\\",\\\"payOnDelivery\\\":\\\"支持\\\",\\\"invoiceDown\\\":\\\"支持\\\",\\\"order_download_limitnum\\\":\\\"\\\",\\\"order_download_limittime\\\":\\\"\\\",\\\"platform_encrypt\\\":\\\"否\\\",\\\"support_decrypt\\\":\\\"\\\",\\\"mergeOrderLogistics\\\":\\\"支持\\\",\\\"splitLogisitics\\\":\\\"支持\\\",\\\"deliverLogistics\\\":\\\"不支持\\\",\\\"minute\\\":\\\"\\\",\\\"splitorderallsend\\\":\\\"支持\\\",\\\"fullsendallitem\\\":\\\"需要\\\",\\\"SupportOnlineExchangeShipping\\\":\\\"支持\\\",\\\"SupportOfflineExchangeShipping\\\":\\\"支持\\\",\\\"SupportOnlineRepairShipping\\\":\\\"支持\\\",\\\"SupportOfflineRepairShipping\\\":\\\"支持\\\",\\\"SplitOrderUploadRemark\\\":\\\"支持\\\",\\\"PushASOrderToJGDPlatform\\\":\\\"不支持\\\",\\\"isNeedTakingTime\\\":\\\"支持\\\",\\\"inventoryUpload\\\":\\\"Uploading\\\",\\\"photePhicull\\\":\\\"支持\\\",\\\"goodsDownload\\\":\\\"goodsCreateOrUpdate\\\",\\\"photoSource1\\\":\\\"线上\\\",\\\"status_Status\\\":\\\"所有状态,上架,下架\\\",\\\"goods_i_id\\\":\\\"不支持\\\",\\\"goods_sku_id\\\":\\\"支持\\\",\\\"goods_color_specifications\\\":\\\"不支持\\\",\\\"goods_price\\\":\\\"支持\\\",\\\"goods_time\\\":\\\"支持\\\",\\\"Iid\\\":\\\"支持\\\",\\\"Link\\\":\\\"支持\\\",\\\"afferSale\\\":\\\"支持\\\",\\\"after_download\\\":\\\"支持\\\",\\\"after_download1\\\":\\\"2\\\",\\\"nodeDownload\\\":\\\"申请退款即下载\\\",\\\"exchangeDewnloa\\\":\\\"支持\\\",\\\"exchangeExclude\\\":\\\"不支持\\\",\\\"after_intercept_order\\\":\\\"支持\\\",\\\"after_intercept_order_count\\\":\\\"支持\\\",\\\"downloadOrderNum\\\":\\\"\\\",\\\"item_amount\\\":\\\"\\\",\\\"after_plat\\\":\\\"不支持\\\",\\\"confirm_good\\\":\\\"不支持\\\",\\\"refund_download_limitnum\\\":\\\"\\\",\\\"refund_download_limittime\\\":\\\"\\\",\\\"IsRefundQuestionTypeMap\\\":\\\"true\\\",\\\"IsOpenAg\\\":\\\"开启\\\",\\\"IsOpenAgLater\\\":\\\"开启\\\",\\\"is_online\\\":\\\"线上\\\",\\\"shopPage_is_common\\\":\\\"定制\\\",\\\"authorizeExplain\\\":\\\"<span style=\\\\\\\"font-weight: bold;\\\\\\\">111</span><img src=\\\\\\\"https://img.alicdn.com/imgextra/i1/50839850/O1CN01LVdOjx2MdLtJoXt4n_!!0-saturn_solar.jpg_468x468q75.jpg_.webp\\\\\\\">\\\",\\\"remark\\\":\\\"<span style=\\\\\\\"font-style: italic;\\\\\\\">222</span><img src=\\\\\\\"https://img.alicdn.com/imgextra/i1/50839850/O1CN01LVdOjx2MdLtJoXt4n_!!0-saturn_solar.jpg_468x468q75.jpg_.webp\\\\\\\">\\\",\\\"syncInvoice\\\":false,\\\"_cbb_syncStatus\\\":\\\"\\\",\\\"_cbb_status_Status\\\":\\\"\\\",\\\"childLogo\\\":[{\\\"LogoName\\\":\\\"淘宝定制\\\",\\\"LogoImg\\\":\\\"\\\",\\\"Labels\\\":\\\"\\\",\\\"ChildkeyWords\\\":\\\"淘宝定制\\\",\\\"SubAppCId\\\":1,\\\"DisplayCoIds\\\":\\\"1\\\",\\\"CreateType\\\":\\\"\\\",\\\"Enabled\\\":true,\\\"IsAuth\\\":true,\\\"BigLogo\\\":\\\"https://files.erp321.com/platform/pic/shoptype/big/淘宝定制.png\\\",\\\"SmallLogo\\\":\\\"https://files.erp321.com/platform/pic/shoptype/small/淘宝定制.png\\\"},{\\\"LogoName\\\":\\\"天猫\\\",\\\"LogoImg\\\":\\\"https://cdn00.erp321.com/jst_contract/2022/09/temp/de144b0860b14a90bde7a07516dfe62d.png\\\",\\\"Labels\\\":\\\"12312312312\\\",\\\"ChildkeyWords\\\":\\\"天猫\\\",\\\"SubAppCId\\\":0,\\\"DisplayCoIds\\\":\\\"\\\",\\\"CreateType\\\":\\\"\\\",\\\"Enabled\\\":true,\\\"IsAuth\\\":true,\\\"BigLogo\\\":\\\"https://files.erp321.com/platform/pic/shoptype/big/天猫.png\\\",\\\"SmallLogo\\\":\\\"https://files.erp321.com/platform/pic/shoptype/small/天猫.png\\\"}]}\",\"enabled\":true,\"co_ids\":\"\",\"status\":\"Online\",\"version\":1,\"developer\":\"123\",\"shop_site\":\"淘宝天猫\",\"shop_type\":\"Taobao\",\"sort\":1,\"is_other\":true,\"key_words\":\"淘宝天猫\",\"type\":\"Import\",\"apicode\":1,\"notify_phone\":\"\",\"remark\":\"<span style=\\\"font-style: italic;\\\">222</span><img src=\\\"https://img.alicdn.com/imgextra/i1/50839850/O1CN01LVdOjx2MdLtJoXt4n_!!0-saturn_solar.jpg_468x468q75.jpg_.webp\\\">\",\"authorize_explain\":\"<span style=\\\"font-weight: bold;\\\">111</span><img src=\\\"https://img.alicdn.com/imgextra/i1/50839850/O1CN01LVdOjx2MdLtJoXt4n_!!0-saturn_solar.jpg_468x468q75.jpg_.webp\\\">\"}";
            var dictoriginal = JsonConvert.DeserializeObject<Dictionary<object, object>>(originalJson);
            dictoriginal.TryGetValue("settings", out object settings);//获取到所有的json的值
            var dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(settings.ToString());
            //取出来固定的信息
            foreach (var item in keys)
            {
                var key = item.Key;//获取到keys
                var value = item.Value.ToString();//获取到keys
                dict.TryGetValue(key, out object valueTxt);
                retDic.Add(value, valueTxt);
            }

            //文本架构
            var settingJson = "[{\"moName\":\"接口授权\",\"moControl\":[{\"coName\":\"Redio\",\"coTitle\":\"是否需要订购\",\"coText\":\"\",\"coAttribute\":{\"name\":\"subscribe\",\"id\":\"subscribe,subscribes\",\"style\":\"\",\"items\":\"需要,不需要\",\"defaltItems\":\"不需要\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"订购金额\",\"coText\":\"\",\"coAttribute\":{\"name\":\"orderMoney\",\"id\":\"orderMoney\",\"style\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[\"number\"]},{\"coName\":\"TextBox\",\"coTitle\":\"服务订购地址\",\"coText\":\"\",\"coAttribute\":{\"name\":\"authExplainUrl\",\"id\":\"authExplainUrl\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"平台返佣金额\",\"coText\":\"\",\"coAttribute\":{\"name\":\"rebateMoney\",\"id\":\"rebateMoney\",\"style\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[\"number\"]},{\"coName\":\"Redio\",\"coTitle\":\"授权方式\",\"coText\":\"\",\"coAttribute\":{\"name\":\"accredit\",\"id\":\"accredit,accredits\",\"style\":\"\",\"items\":\"账号登录式授权,key+secrcet\",\"defaltItems\":\"key+secrcet\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"授权说明链接\",\"coText\":\"\",\"coAttribute\":{\"name\":\"auth_explain_url\",\"id\":\"auth_explain_url\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"授权手册\",\"coText\":\"\",\"coAttribute\":{\"name\":\"authorization_anual\",\"id\":\"authorization_anual\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"开关说明\",\"coText\":\"\",\"coAttribute\":{\"name\":\"switch_url\",\"id\":\"switch_ur\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"掌柜昵称说明链接\",\"coText\":\"\",\"coAttribute\":{\"name\":\"nick_explain_url\",\"id\":\"nick_explain_url\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"SelectOpion\",\"coTitle\":\"授权准确性校验\",\"coText\":\"\",\"coAttribute\":{\"name\":\"isAccurate\",\"id\":\"isAccurate\",\"style\":\"width:240px\",\"disabled\":\"true\",\"itemsId\":\"\",\"items\":\"Order,订单下载(时间);GetOrder,订单下载(按单号);OrderCount,订单下载(总数);Item,商品下载(时间);GetItem,商品下载(按款);Refund,售后单下载;Inventory,库存同步;Shipping,发货回传;noSupport,不支持\",\"defaltItems\":\"noSupport,不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"api地址\",\"coText\":\"\",\"coAttribute\":{\"name\":\"api_url\",\"id\":\"api_url\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"是否为永久授权\",\"coText\":\"\",\"coAttribute\":{\"name\":\"permanentAuthorization\",\"id\":\"permanentAuthorization,permanentAuthorizations\",\"style\":\"\",\"items\":\"是,否\",\"defaltItems\":\"否\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"授权互通平台\",\"coText\":\"\",\"coAttribute\":{\"name\":\"UseSameAuthOtherPlatform\",\"id\":\"UseSameAuthOtherPlatform\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"支持面单店铺\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsSupportWayBillShop\",\"id\":\"SupportWayBillShop,UnSupportWayBillShop\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"默认appcid\",\"coText\":\"0\",\"coAttribute\":{\"name\":\"DefaultAppCid\",\"id\":\"DefaultAppCid\",\"style\":\"width:140px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"SessionUId页面名称\",\"coText\":\"0\",\"coAttribute\":{\"name\":\"SessionUIdTextName\",\"id\":\"SessionUIdTextName\",\"style\":\"width:200px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"奇门绑定面单店铺校验订单接口\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsBindWayBillValidOrder\",\"id\":\"BindWayBillValidOrder,NotBindWayBillValidOrder\",\"style\":\"\",\"items\":\"校验,不校验\",\"defaltItems\":\"校验\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio()\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"是否为标准授权页面\",\"coText\":\"\",\"coAttribute\":{\"name\":\"isStandardKeySecrcet\",\"id\":\"isStandardKeySecrcet,isStandardKeySecrcets\",\"style\":\"\",\"items\":\"是,否\",\"defaltItems\":\"否\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"授权过期时间是否显示为RefreshTokenExpired\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsShowRefreshTokenExpired\",\"id\":\"IsShowRefreshTokenExpired,IsShowRefreshTokenExpireds\",\"style\":\"\",\"items\":\"是,否\",\"defaltItems\":\"否\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"ERP授权发起页面域名\",\"coText\":\"0\",\"coAttribute\":{\"name\":\"AuthRequestUrl\",\"id\":\"AuthRequestUrl\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]}]},{\"moName\":\"订单\",\"moControl\":[{\"coName\":\"TextBox\",\"coTitle\":\"订单同步方式\",\"coText\":\"\",\"coAttribute\":{\"name\":\"indentSync\",\"id\":\"indentSync\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"true\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"订单手工下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"order_download,order_downloads\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio7()\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"订单手工下载最长多久(天)\",\"coText\":\"\",\"coAttribute\":{\"name\":\"order_download1\",\"id\":\"order_download1\",\"style\":\"width: 240px\",\"itemsId\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"true\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"最长可拉取多长范围订单\",\"coText\":\"\",\"coAttribute\":{\"name\":\"longestTime\",\"id\":\"longestTime\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"按创建时间or更新时间下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"create_modify\",\"id\":\"create_modify\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"订单商品图片下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"syncPhotoDownload\",\"id\":\"syncPhotoDownload,syncPhotoDownloads\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio()\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"订单图片来源\",\"coText\":\"\",\"coAttribute\":{\"name\":\"photoSource\",\"id\":\"photoSource\",\"style\":\"width:240px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"true\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"款式编码\",\"coText\":\"\",\"coAttribute\":{\"name\":\"order_i_id\",\"id\":\"order_i_id,order_i_ids\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"商品编码\",\"coText\":\"\",\"coAttribute\":{\"name\":\"order_sku_id\",\"id\":\"order_sku_id,order_sku_ids\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"颜色规格\",\"coText\":\"\",\"coAttribute\":{\"name\":\"color_specifications\",\"id\":\"color_specifications,color_specifications\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"SelectOpion\",\"coTitle\":\"拆单规则\",\"coText\":\"\",\"coAttribute\":{\"name\":\"resolution\",\"id\":\"resolution\",\"style\":\"width:240px\",\"disabled\":\"true\",\"itemsId\":\"\",\"items\":\"goodsNoNumber,可拆分商品，不可拆分数量;goodsNumber,可拆分商品和数量;noGoodsNumber,不支持\",\"defaltItems\":\"noGoodsNumber,不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextOpion\",\"coTitle\":\"可同步订单状态\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"syncOrderStatus\",\"style\":\"\",\"itemsId\":\"syncStatus\",\"items\":\"待付款,已付款,已发货,关闭,取消\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"订单分摊逻辑从数据组计算\",\"coText\":\"\",\"coAttribute\":{\"name\":\"order_apportions\",\"id\":\"order_apportion,order_apportions\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"订单成本价\",\"coText\":\"\",\"coAttribute\":{\"name\":\"costPrice\",\"id\":\"costPrice,costPrices\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio6()\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"店铺设置标题\",\"coText\":\"\",\"coAttribute\":{\"name\":\"costPriceTitle\",\"id\":\"costPriceTitle\",\"style\":\"width:240px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"true\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"订单金额为\",\"coText\":\"\",\"coAttribute\":{\"name\":\"indentMoney\",\"id\":\"indentMoney\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"订单下载提示内容\",\"coText\":\"\",\"coAttribute\":{\"name\":\"order_down_content\",\"id\":\"order_down_content\",\"style\":\"width:610px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"卖家备注下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"sellerDemarkDowmload\",\"id\":\"sellerDemarkDowmload,sellerDemarkDowmloads\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"旗帜上传\",\"coText\":\"\",\"coAttribute\":{\"name\":\"flag_upload\",\"id\":\"flag_upload,flag_uploads\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"旗帜下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"flag_down\",\"id\":\"flag_down,flag_downs\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"卖家备注上传\",\"coText\":\"\",\"coAttribute\":{\"name\":\"sellerUploading\",\"id\":\"sellerUploading,sellerUploadings\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"买家留言下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"sellerMessageDowmload\",\"id\":\"sellerMessageDowmload,sellerMessageDowmloads\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"线上改地址同步下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"onlineAdressDown\",\"id\":\"onlineAdressDown,onlineAdressDowns\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"erp改地址同步上传\",\"coText\":\"\",\"coAttribute\":{\"name\":\"erpAdressUploading\",\"id\":\"erpAdressUploading,erpAdressUploadings\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"SelectOpion\",\"coTitle\":\"线上退款状态同步\",\"coText\":\"\",\"coAttribute\":{\"name\":\"onlineRefundStatus\",\"id\":\"onlineRefundStatus\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"notSupport,不支持;applyIntercept,申请拦截;agreeRefundIntercept,同意退款后拦截\",\"defaltItems\":\"notSupport,不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"线上取消退款同步\",\"coText\":\"\",\"coAttribute\":{\"name\":\"offlineCancelRefund\",\"id\":\"offlineCancelRefund,offlineCancelRefunds\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"线上快递同步erp\",\"coText\":\"\",\"coAttribute\":{\"name\":\"express_delivery\",\"id\":\"express_delivery,express_deliverys\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"货到付款\",\"coText\":\"\",\"coAttribute\":{\"name\":\"payOnDelivery\",\"id\":\"payOnDelivery,payOnDeliverys\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"发票信息下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"invoiceDown\",\"id\":\"invoiceDown,invoiceDowns\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"订单下载最大个数\",\"coText\":\"\",\"coAttribute\":{\"name\":\"order_download_limitnum\",\"id\":\"order_download_limitnum\",\"style\":\"width:100px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"订单下载最长时间\",\"coText\":\"\",\"coAttribute\":{\"name\":\"order_download_limittime\",\"id\":\"order_download_limittime\",\"style\":\"width:100px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"平台是否加密\",\"coText\":\"\",\"coAttribute\":{\"name\":\"platform_encrypt,platform_encrypts\",\"id\":\"platform_encrypt,platform_encrypts\",\"style\":\"\",\"items\":\"是,否\",\"defaltItems\":\"否\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextArea\",\"coTitle\":\"是否支持解密\",\"coText\":\"\",\"coAttribute\":{\"name\":\"support_decrypt\",\"id\":\"support_decrypt\",\"style\":\"width:610px;height:90px;\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]}]},{\"moName\":\"发货\",\"moControl\":[{\"coName\":\"Redio\",\"coTitle\":\"合并订单物流同步\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"mergeOrderLogistics,mergeOrderLogisticss\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"拆单物流同步\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"splitLogisitics,splitLogisiticss\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"发货后修改物流单号\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"deliverLogistics,deliverLogisticss\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio2()\",\"colimit\":[\"number\"]},{\"coName\":\"TextBox\",\"coTitle\":\"发货后多长时间内修改\",\"coText\":\"\",\"coAttribute\":{\"name\":\"minute\",\"id\":\"minute\",\"style\":\"width: 120px\",\"itemsId\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"true\",\"coHidden\":\"true\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"拆分订单支持整单发货\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"splitorderallsend,unsplitorderallsend\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio2()\",\"colimit\":[\"number\"]},{\"coName\":\"Redio\",\"coTitle\":\"整单发货需要所有商品\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"fullsendallitem,unfullsendallitem\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"需要,不需要\",\"defaltItems\":\"不需要\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio2()\",\"colimit\":[\"number\"]},{\"coName\":\"Redio\",\"coTitle\":\"线上换货订单发货\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsSupportOnlineExchangeShipping\",\"id\":\"SupportOnlineExchangeShipping,NotSupportOnlineExchangeShipping\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio()\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"线下换货订单发货\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsSupportOfflineExchangeShipping\",\"id\":\"SupportOfflineExchangeShipping,NotSupportOfflineExchangeShipping\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio()\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"线上补发订单发货\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsSupportOnlineRepairShipping\",\"id\":\"SupportOnlineRepairShipping,NotSupportOnlineRepairShipping\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio()\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"线下补发订单发货\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsSupportOfflineRepairShipping\",\"id\":\"SupportOfflineRepairShipping,NotSupportOfflineRepairShipping\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio()\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"备注上传拆分订单\",\"coText\":\"\",\"coAttribute\":{\"name\":\"SplitOrderUploadRemark\",\"id\":\"SplitOrderUploadRemark,SplitOrderUploadRemark\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"推送售后发货到聚工单\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"PushASOrderToJGDPlatform,UnPushASOrderToJGDPlatform\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"依赖轨迹发货\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"isNeedTakingTime,unisNeedTakingTime\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]}]},{\"moName\":\"库存\",\"moControl\":[{\"coName\":\"SelectOpion\",\"coTitle\":\"库存上传模式\",\"coText\":\"\",\"coAttribute\":{\"name\":\"inventoryUpload\",\"id\":\"inventoryUpload\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"Uploading,全量上传;Uploadings,增量上传;NoUploadings,不支持\",\"defaltItems\":\"NoUploadings,不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]}]},{\"moName\":\"商品\",\"moControl\":[{\"coName\":\"Redio\",\"coTitle\":\"商品图片下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"photePhicull,photePhiculls\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio3()\",\"colimit\":[]},{\"coName\":\"SelectOpion\",\"coTitle\":\"商品下载规则\",\"coText\":\"\",\"coAttribute\":{\"name\":\"goodsDownload\",\"id\":\"goodsDownload\",\"style\":\"width:240px\",\"itemsId\":\"\",\"items\":\"goodsCreate,创建时间;goodsUpdate,更新时间;goodsNoTime,无时间全量下载;goodsCreateOrUpdate,创建或更新;nooodsDownload,不支持\",\"defaltItems\":\"goodsUpdate,更新时间\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"商品图片来源\",\"coText\":\"\",\"coAttribute\":{\"name\":\"photoSource1\",\"id\":\"photoSource1\",\"style\":\"width: 240px\",\"itemsId\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"true\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextOpion\",\"coTitle\":\"商品按状态下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"status_all\",\"style\":\"\",\"itemsId\":\"status_Status\",\"items\":\"上架,下架,不支持\",\"defaltItems\":\"上架\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"商家款式编码\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"goods_i_id,goods_i_ids\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"商家商品编码\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"goods_sku_id,goods_sku_ids\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"颜色规格\",\"coText\":\"\",\"coAttribute\":{\"name\":\"goods_color_specifications\",\"id\":\"goods_color_specifications,goods_color_specifications\",\"style\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"商品销售价格\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"goods_price,goods_prices\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"商品按时间下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"goods_time,goods_times\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"商品支持按款下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"Iid,Iids\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio3()\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"商品支持链接下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"Link,Links\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio3()\",\"colimit\":[]}]},{\"moName\":\"售后\",\"moControl\":[{\"coName\":\"Redio\",\"coTitle\":\"下载售后单\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"afferSale,afferSales\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio4()\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"售后单手工下载\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"after_download,after_downloads\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"editRadio8()\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"手工下载售后单最长多久(天)\",\"coText\":\"\",\"coAttribute\":{\"name\":\"after_download1\",\"id\":\"after_download1\",\"style\":\"width: 240px\",\"itemsId\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"true\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"下载节点\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"nodeDownload,nodeDownloads\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"申请退款即下载,同意退款后下载\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"true\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"下载换货单\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"exchangeDewnloa,exchangeDewnloas\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"排除换货单\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"exchangeExclude,exchangeExcludes\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"拦截订单默认启用\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"after_intercept_order, after_intercept_orders\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"拦截订单部分退数量\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"after_intercept_order_count, after_intercept_order_counts\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"SelectOpion\",\"coTitle\":\"售后单下载按单号\",\"coText\":\"\",\"coAttribute\":{\"name\":\"downloadOrderNum\",\"id\":\"downloadOrderNum\",\"style\":\"width:240px\",\"itemsId\":\"\",\"items\":\"downloadOrderNum,线上订单号; downloadOrderNums,线上售后单号; nodownloadOrderNum,不支持\",\"defaltItems\":\"downloadOrderNums,线上售后单号\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"售后单按createORupdate\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"cratetime,updatetime\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"明细退款金额(线上)\",\"coText\":\"\",\"coAttribute\":{\"name\":\"item_amount\",\"id\":\"item_amount1\",\"style\":\"width: 240px\",\"itemsId\":\"\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"售后单审核回传平台\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"after_plat,after_plats\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"确认收到货物回传平台\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"confirm_good,confirm_goods\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"支持,不支持\",\"defaltItems\":\"不支持\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"售后下载最大个数\",\"coText\":\"\",\"coAttribute\":{\"name\":\"refund_download_limitnum\",\"id\":\"refund_download_limitnum\",\"style\":\"width:100px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"TextBox\",\"coTitle\":\"售后下载最长时间\",\"coText\":\"\",\"coAttribute\":{\"name\":\"refund_download_limittime\",\"id\":\"refund_download_limittime\",\"style\":\"width:100px\",\"items\":\"\",\"defaltItems\":\"\"},\"coMust\":\"false\",\"colimit\":[]},{\"coName\":\"SelectOpion\",\"coTitle\":\"售后问题类型是否映射\",\"coText\":\"\",\"coAttribute\":{\"name\":\"IsRefundQuestionTypeMap\",\"id\":\"IsRefundQuestionTypeMap\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"true,是;false,否\",\"defaltItems\":\"false,否\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"开通AG\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"IsOpenAg,OpenAg\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"开启,关闭\",\"defaltItems\":\"关闭\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"开通AG延迟执行\",\"coText\":\"\",\"coAttribute\":{\"name\":\"\",\"id\":\"IsOpenAgLater,OpenAgLater\",\"style\":\"\",\"itemsId\":\"\",\"items\":\"开启,关闭\",\"defaltItems\":\"关闭\"},\"coMust\":\"false\",\"coHidden\":\"false\",\"eventName\":\"\",\"colimit\":[]}]},{\"moName\":\"店铺\",\"moControl\":[{\"coName\":\"Redio\",\"coTitle\":\"所属渠道\",\"coText\":\"\",\"coAttribute\":{\"name\":\"isonline\",\"id\":\"is_online,offline\",\"style\":\"\",\"items\":\"线上,线下\",\"defaltItems\":\"线上\"},\"coMust\":\"true\",\"colimit\":[]},{\"coName\":\"Redio\",\"coTitle\":\"店铺配置\",\"coText\":\"\",\"coAttribute\":{\"name\":\"iscommon\",\"id\":\"shopPage_is_common,custom\",\"style\":\"\",\"items\":\"通用,定制\",\"defaltItems\":\"通用\"},\"coMust\":\"true\",\"colimit\":[]}]}]";
            var shopSiteList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ShopSiteEditModel>>(settingJson);

            foreach (var shopSite in shopSiteList)
            {
                foreach (var item in shopSite.ModelControls)
                {
                    var ids = item.ControlAttributes.AttributeId.Split(',');
                    if (ids.Count() > 0)
                    {
                        foreach (var id in ids)
                        {
                            var values = dict.GetValueOrDefault(id);
                            if (values != null)
                            {
                                if (!retDic.TryGetValue(item.ControlTitle, out object strpara))
                                {
                                    retDic.Add(item.ControlTitle, values);
                                }
                            }
                        }
                    }
                }
            }



            var retJson = JsonConvert.SerializeObject(retDic);

            Console.ReadKey();
        }

    }


    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string sync_order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sync_item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sync_inventory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sync_delivery { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string after_sale_service_download { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sync_invoice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int token_expire_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string auto_refresh_token { get; set; }
        /// <summary>
        /// {"id":"904","hiddenEditModel":"add","shop_site":"Akulaku","shop_type":"Akulaku","contract_content":"","imgid":"","logo_sort":"","type":"CrossBorder","sort":"0","status":"Development","onlineDate":"2022-04-27","version":"1","enabled":"true","co_ids":"","syncOrder":true,"syncItem":true,"syncInventory":true,"syncDelivery":true,"serviceDownload":true,"developer":"芜菁","developer1":"天冬","apicode":"0","key_words":"Akulaku","tokenExpireIn":"永久有效","autoRefreshToken":"false","refreshTokenExpireIn":0,"relationWay":"","subscribe":"需要","orderMoney":"12121","authExplainUrl":"","rebateMoney":"","accredit":"key+secrcet","auth_explain_url":"https://ww.erp321.com/app/support/document.html#page=3747","authorization_anual":"","switch_url":"","nick_explain_url":"","isAccurate":"","api_url":"","permanentAuthorization":"否","UseSameAuthOtherPlatform":"","SupportWayBillShop":"支持","DefaultAppCid":"111","SessionUIdTextName":"","BindWayBillValidOrder":"校验","isStandardKeySecrcet":"否","IsShowRefreshTokenExpired":"否","indentSync":"手动下载","order_download":"支持","order_download1":"99999999","longestTime":"","create_modify":"","syncPhotoDownload":"不支持","photoSource":"","order_i_id":"不支持","order_sku_id":"支持","color_specifications":"不支持","resolution":"","syncStatus":"待付款,已付款,已发货,关闭,取消","order_apportion":"不支持","costPrice":"不支持","costPriceTitle":"","indentMoney":"","order_down_content":"","sellerDemarkDowmload":"不支持","flag_upload":"不支持","flag_down":"不支持","sellerUploading":"不支持","sellerMessageDowmload":"不支持","onlineAdressDown":"不支持","erpAdressUploading":"不支持","onlineRefundStatus":"","offlineCancelRefund":"不支持","express_delivery":"支持","payOnDelivery":"不支持","invoiceDown":"不支持","order_download_limitnum":"","order_download_limittime":"","platform_encrypt":"否","support_decrypt":"","mergeOrderLogistics":"不支持","splitLogisitics":"不支持","deliverLogistics":"不支持","minute":"","splitorderallsend":"不支持","fullsendallitem":"不需要","SupportOnlineExchangeShipping":"不支持","SupportOfflineExchangeShipping":"不支持","SupportOnlineRepairShipping":"不支持","SupportOfflineRepairShipping":"不支持","SplitOrderUploadRemark":"不支持","PushASOrderToJGDPlatform":"不支持","isNeedTakingTime":"不支持","inventoryUpload":"Uploading","photePhicull":"不支持","goodsDownload":"","photoSource1":"","status_Status":"所有状态,上架,下架","goods_i_id":"不支持","goods_sku_id":"支持","goods_color_specifications":"不支持","goods_price":"支持","goods_time":"支持","Iid":"不支持","Link":"不支持","afferSale":"不支持","after_download":"不支持","after_download1":"","exchangeDewnloa":"不支持","exchangeExclude":"不支持","downloadOrderNum":"","item_amount":"","after_plat":"不支持","confirm_good":"不支持","refund_download_limitnum":"","refund_download_limittime":"","IsRefundQuestionTypeMap":"","IsOpenAg":"关闭","IsOpenAgLater":"关闭","is_online":"线上","shopPage_is_common":"通用","authorizeExplain":"","remark":"","is_other":"","syncInvoice":false,"_cbb_syncStatus":"","_cbb_status_Status":"","nodeDownload":""}
        /// </summary>
        public string settings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string co_ids { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 芜菁
        /// </summary>
        public string developer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shop_site { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shop_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_other { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string key_words { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int apicode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string notify_phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string authorize_explain { get; set; }
    }

    public class ShopSiteEditModel
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        [JsonProperty("moName")]
        public string ModelName { get; set; }

        /// <summary>
        /// 控件集合
        /// </summary>
        [JsonProperty("moControl")]
        public List<ShopSiteEditControl> ModelControls { get; set; }
    }

    public class ShopSiteEditControl
    {
        /// <summary>
        /// 控件名称
        /// </summary>
        [JsonProperty("coName")]
        public string ControlName { get; set; }

        /// <summary>
        /// 控件标题
        /// </summary>
        [JsonProperty("coTitle")]
        public string ControlTitle { get; set; }

        /// <summary>
        /// 控件文本值
        /// </summary>
        [JsonProperty("coText")]
        public string ControlText { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [JsonProperty("coMust")]
        public string ControlMust { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [JsonProperty("coHidden")]
        public string ControlHidden { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        [JsonProperty("eventName")]
        public string EventName { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [JsonProperty("coAttribute")]
        public ShopSiteAttribute ControlAttributes { get; set; }

        /// <summary>
        /// 控件输入限制
        /// </summary>
        [JsonProperty("colimit")]
        public string[] ControlLimits { get; set; }
    }

    public class ShopSiteAttribute
    {
        /// <summary>
        /// 属性name
        /// </summary>
        [JsonProperty("name")]
        public string AttributeName { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        [JsonProperty("id")]
        public string AttributeId { get; set; }

        /// <summary>
        /// 属性style
        /// </summary>
        [JsonProperty("style")]
        public string AttributeStyle { get; set; }

        /// <summary>
        /// 数据集id
        /// </summary>
        [JsonProperty("itemsId")]
        public string AttributeItemsId { get; set; }

        /// <summary>
        /// 下拉框、等一些集合控件--字典数据
        /// </summary>
        [JsonProperty("items")]
        public string AttributeItems { get; set; }

        /// <summary>
        /// 下拉框、等一些集合控件--字典数据(默认显示)
        /// </summary>
        [JsonProperty("defaltItems")]
        public string AttributeDefaltItems { get; set; }
    }
}
