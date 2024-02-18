using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities;

//Todo: 2.6.1 Create Entity
public class BaseEntity
{
    /// <summary>
    ///Trong C#, hai attribute [BsonId] và [BsonRepresentation(BsonType.ObjectId)] thường được sử dụng khi làm việc với MongoDB1.
    ///[BsonId]: Đánh dấu thuộc tính làm khóa chính (primary key) cho một đối tượng trong MongoDB2.
    /// 
    ///[BsonRepresentation(BsonType.ObjectId)]: Chỉ định cách mà giá trị của thuộc tính được biểu diễn khi lưu trữ và truy vấn32.
    /// Trong trường hợp này, BsonType.ObjectId cho biết giá trị của thuộc tính sẽ được biểu diễn dưới dạng ObjectId của MongoDB
    ///
    /// Trong đó, Id là khóa chính và được biểu diễn dưới dạng ObjectId khi lưu trữ và truy vấn2.
    /// Tuy nhiên, trong mã C# của bạn, bạn có thể làm việc với Id như một chuỗi2.
    /// Điều này giúp việc làm việc với MongoDB trở nên dễ dàng hơn trong .NET2.
    ///
    /// Trong MongoDB, ObjectId là một định danh duy nhất gồm 12 byte123. Cấu trúc của ObjectId bao gồm:
    ///     4 byte đầu tiên biểu diễn thời gian tạo ObjectId, được đo bằng giây kể từ thời điểm bắt đầu Unix (Unix epoch)123.
    ///     5 byte tiếp theo là giá trị ngẫu nhiên, được tạo một lần cho mỗi quá trình123. Giá trị ngẫu nhiên này là duy nhất cho máy và quá trình1.
    ///     3 byte cuối cùng là một bộ đếm tăng dần, được khởi tạo với một giá trị ngẫu nhiên123.
    ///ObjectId thường được sử dụng để tạo các định danh duy nhất cho tất cả các tài liệu trong cơ sở dữ liệu4.
    /// Nó khác với ID tự tăng truyền thống, nhưng mang lại một số lợi ích riêng4.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}