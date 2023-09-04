using System;
namespace Core.Business.ViewModels
{
    public static class ValidatorMessage
    {
        public static class General
        {
			public static readonly string NotExist = "Dữ liệu không tồn tại";
			public static readonly string NotDestroy = "Dữ liệu đã được sử dụng, không thể xoá";
            public static readonly string ConcurrencyStamp = "Dữ liệu đã được người khác sử dụng, vui lòng refresh lại trang";
			public static readonly string NameNotEmpty = "Tên không được để trống";
			public static readonly string CodeNotEmpty = "Mã không được để trống";
			public static readonly string UniqueName = "Tên đã tồn tại";
			public static readonly string UniqueCode = "Mã đã tồn tại";
			public static readonly string EmailInvalid = "Email không đúng định dạng";
        }

        public static class Account
        {
            public static readonly string NotExist = "Tài khoản không tồn tại";
			public static readonly string UserNameNotEmpty = "Tên đăng nhập không được để trống";
			public static readonly string FullNameNotEmpty = "Họ tên không được để trống";
            public static readonly string EmailInvalid = "Email không đúng định dạng";
            public static readonly string UniqueUserName = "Tên đăng nhập đã tồn tại";
            public static readonly string CodeNotEmpty = "Mã không được để trống";
            public static readonly string UniqueCode = "Mã đã tồn tại";
            public static readonly string PassWordNotEmpty = "Mật khẩu không được để trống";
            public static readonly string InvalidUserNamePassWord = "Tên đăng nhập hoặc mật khẩu không chính xác";
			public static readonly string AccountHasBeenBlock = "Tài khoản của bạn đã bị khoá";
			public static readonly string IdentityCardInvalid = "CMND không hợp lệ";
			public static readonly string CurrentPassWordNotEmpty = "Mật khẩu hiện tại không được để trống";
			public static readonly string CurrentPassWordInValid = "Mật khẩu hiện tại không chính xác";
			public static readonly string NewPassWordNotEmpty = "Mật khẩu mới không được để trống";
        }

		public static class Department
		{
            public static readonly string NotExist = "Phòng ban không tồn tại";
			public static readonly string NameNotEmpty = "Tên phòng ban không được để trống";
			public static readonly string CodeNotEmpty = "Mã phòng ban không được để trống";
			public static readonly string UniqueName = "Tên phòng ban đã tồn tại";
			public static readonly string UniqueCode = "Mã phòng ban đã tồn tại";
		}

        public static class Hub
        {
			public static readonly string NotExist = "Hub không tồn tại";
			public static readonly string CenterNotExist = "Trung tâm không tồn tại";
			public static readonly string PONotExist = "Chi nhánh không tồn tại";
            public static readonly string NameNotEmpty = "Tên Hub không được để trống";
            public static readonly string CodeNotEmpty = "Mã Hub không được để trống";
			public static readonly string UniqueName = "Tên Hub đã tồn tại";
			public static readonly string UniqueCode = "Mã Hub đã tồn tại";
        }

        public static class StationHub
        {
            public static readonly string NotExist = "Trạm không tồn tại";
            public static readonly string NotEmpty = "Trạm không được để trống";
        }

		public static class Role
		{
            public static readonly string NotExist = "Chức vụ không tồn tại";
            public static readonly string NameNotEmpty = "Tên chức vụ không được để trống";
            public static readonly string CodeNotEmpty = "Mã chức vụ không được để trống";
			public static readonly string UniqueName = "Tên chức vụ đã tồn tại";
			public static readonly string UniqueCode = "Mã chức vụ đã tồn tại";
		}

		public static class Country
		{
			public static readonly string NotExist = "Quốc gia không tồn tại";
			public static readonly string NameNotEmpty = "Tên quốc gia không được để trống";
			public static readonly string CodeNotEmpty = "Mã quốc gia không được để trống";
			public static readonly string UniqueName = "Tên quốc gia đã tồn tại";
			public static readonly string UniqueCode = "Mã quốc gia đã tồn tại";
		}

		public static class Province
		{
			public static readonly string NotExist = "Tỉnh/thành không tồn tại";
			public static readonly string NameNotEmpty = "Tên tỉnh/thành không được để trống";
			public static readonly string CodeNotEmpty = "Mã tỉnh/thành không được để trống";
			public static readonly string UniqueName = "Tên tỉnh/thành đã tồn tại";
			public static readonly string UniqueCode = "Mã tỉnh/thành đã tồn tại";
		}

		public static class District
		{
			public static readonly string NotExist = "Quận/huyện không tồn tại";
			public static readonly string NameNotEmpty = "Tên quận/huyện không được để trống";
			public static readonly string CodeNotEmpty = "Mã quận/huyện không được để trống";
			public static readonly string UniqueName = "Tên quận/huyện đã tồn tại";
			public static readonly string UniqueCode = "Mã quận/huyện đã tồn tại";
		}

		public static class Ward
		{
			public static readonly string NotExist = "Phường/xã không tồn tại";
            public static readonly string WardListNotEmpty = "Chưa chọn quận/huyện";
			public static readonly string NameNotEmpty = "Tên phường/xã không được để trống";
			public static readonly string CodeNotEmpty = "Mã phường/xã không được để trống";
			public static readonly string UniqueName = "Tên phường/xã đã tồn tại";
			public static readonly string UniqueCode = "Mã phường/xã đã tồn tại";
		}


        public static class AreaGroup
        {
            public static readonly string NoExist = "Nhóm khu vực không tồn tại";
            public static readonly string CodeNotEmpty = "Mã nhóm khu vực không được để trống";
            public static readonly string UniqueCode = "Mã nhóm khu vực đã tồn tại";
            public static readonly string NameNotEmpty = "Têm nhóm khu vực không được để trống";
            public static readonly string UniqueName = "Tên nhóm khu vực đã tồn tại";
        }

        public static class Area
        {
            public static readonly string NoExist = "Khu vực không tồn tại";
            public static readonly string CodeNotEmpty = "Mã khu vực không được để trống";
            public static readonly string UniqueCode = "Mã khu vực đã tồn tại";
            public static readonly string NameNotEmpty = "Têm khu vực không được để trống";
            public static readonly string UniqueName = "Tên khu vực đã tồn tại";
            public static readonly string DistrictIds = "Quận / huyện không được để trống";
        }

        public static class Weight
        {
            public static readonly string NoExist = "Mức cân không tồn tại";
            public static readonly string CodeNotEmpty = "Mã mức cân không được để trống";
            public static readonly string UniqueCode = "Mã mức cân đã tồn tại";
            public static readonly string WeightFromNotEmpty = "Mức cân bắt đầu không được để trống";
            public static readonly string WeightFrom = "Mức cân bắt đầu phải là số";
            public static readonly string WeightToNotEmpty = "Mức cân đến không được để trống";
            public static readonly string WeightTo = "Mức cân đến phải là số";
            public static readonly string WeightPlusNotEmpty = "Mức cân cộng thêm không được để trống";
            public static readonly string WeightPlus = "Mức cân cộng thêm phải là số";
            public static readonly string FormulaId = "Công thức không được để trống";
            public static readonly string FormulaExist = "Công thức không tồn tại";
        }

        public static class PriceList
        {
            public static readonly string NoExist = "Mức cân không tồn tại";
            public static readonly string CodeNotEmpty = "Mã bảng giá không được để trống";
            public static readonly string UniqueCode = "Mã bảng giá đã tồn tại";
            public static readonly string NameNotEmpty = "Têm bảng giá không được để trống";
            public static readonly string UniqueName = "Tên bảng giá đã tồn tại";
        }

        public static class Formula
        {
            public static readonly string NoExist = "Công thức không tồn tại";
        }

        public static class Calculator
        {
            public static readonly string NotFoundPriceList = "Không tìm thấy bảng giá";
            public static readonly string NotFoundPriceListInArea = "Khu vực chưa cài đặt bảng giá công bố";
            public static readonly string NotFoundService = "Không tim thấy dịch vụ";
            public static readonly string NotFoundPriceService = "Dịch vụ không có giá";
            public static readonly string NotFoundAreaGroup = "Không tìm thấy nhóm khu vực tính giá theo dịch vụ";
            public static readonly string NotFoundArea = "Không tìm thây khu vực tính giá theo quận huyện";
            public static readonly string EmptyWeight = "Không tìm thấy mức cân tính giá";
            public static readonly string NotFoundServicePrice = "Không tìm thấy mức giá cho dịch vụ này";
            public static readonly string NotFoundFormula = "Không tìm thấy công thức tính giá";
            public static readonly string NotFoundHubFrom = "Không xác định được TT/CN/TRẠM gửi";
            public static readonly string SetupWeightPlusError = "Mức cân cộng thêm bắt đầu không khớp với mức cân trước đó";
            public static readonly string ServicePriceZero = "Giá dịch vụ vận chuyển 0";
        }

        public static class UploadExcelPrice
        {
            public static readonly string AreaCodesNotEmpty = "Mã khu vực không được trống";
            public static readonly string AreaGroupNotEmpty = "Nhóm khu vực không được trống";
            public static readonly string WeightCodesNotEmpty = "Mã mức cân không được trống";
            public static readonly string WeightGroupNotEmpty = "Nhóm mức cân không được trống";
            public static readonly string PriceServiceNotEmpty = "Vui lòng chọn bảng giá dịch vụ";
            public static readonly string AreaCodeNotExist = "Mã khu vực tính giá {0} không phù hợp";
            public static readonly string WeightCodeNotExist = "Mã mức cân {0} không phù hợp";
            public static readonly string PriceServiceDetailNotEmpty = "Không tìm thấy thông tin giá dịch vụ";
        }

        public static class ProvideCode
        {
            public static readonly string CreateError = "Cấp mới vận đơn không thành công.";
            public static readonly string ProvideCodeExist = "Số vận đơn đã tồn tại trong danh sách đã cấp.";
            public static readonly string ProvideCodeExistInShipment = "Số vận đơn đã tồn tại trong danh sách vận đơn.";
            public static readonly string NumberStartAdnEndInValid = "Số bắt đầu và đến không hợp lệ.";
            public static readonly string ProvideCodeUsed = "Số vận đơn đã được sử dụng.";
            public static readonly string ProvideCountError = "Số vận đơn tự sinh không hợp lệ.";
            public static readonly string DataError = "Dữ liệu lỗi.";
        }

        public static class UploadExcelDeadline
        {
            public static readonly string ServiceCodesNotEmpty = "Mã dịch vụ không được trống";
            public static readonly string DeadlinePickupDeliveryNotEmpty = "Vui lòng chọn deadline cần thao tác";
            public static readonly string SystemNotEmpty = "Hệ thống chưa có dịch vụ";
            public static readonly string WeightServiceNotExist = "Mã dịch vụ {0} không phù hợp";
            public static readonly string DeadlineNotEmpty = "Không tìm thấy thông tin deadline";

        }

        public static class DeadlinePickupDelivery
        {
            public static readonly string TimeStartNotEmpty = "Thời gian tạo/nhận đơn hàng không được trống.";
            public static readonly string FromWardNotEmpty = "Phường xã nhận không được trống.";
            public static readonly string FromHubNotEmpty = "TT/CN/TRẠM nhận không được trống.";
            public static readonly string FromHubIsValid = "Không xác định TT/CN/TRẠM nhận.";
            public static readonly string FromHubNotDeadline = "Không tìm thấy Deadline theo TT/CN/TRẠM nhận.";
            public static readonly string ServiceNotFound = "Không tìm thấy dịch vụ";
            public static readonly string ToDistrictNotEmpty = "Quận huyện phát không được trống";
            public static readonly string DeadlineDetailNotFound = "Deadline chi tiết không có";
        }
    }
}
