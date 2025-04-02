MindsetCRM - Mikroservis Tabanlı CRM Uygulaması

📄 Proje Hakkında
Bu proje; kullanıcı yönetimi, müşteri takibi ve satış sürecini yöneten modern bir CRM uygulamasıdır. Mikroservis mimarisi ile geliştirilmiş, her servis bağımsız çalışacak şekilde tasarlanmıştır. API Gateway ile servisler bir araya getirilmiştir. Proje .NET 8 ve Docker altyapısıyla inşa edilmiştir.

🧰 Mimari Yapı
Mikroservisler: UserService, CustomerService, SalesService
API Gateway: Ocelot
Veritabanları:
UserService & CustomerService: SQL Server
SalesService: MongoDB
Kimlik Doğrulama: JWT
İletişim: Mikroservisler arası HTTP (API Gateway aracılığıyla)
Loglama: Serilog ile merkezi loglama (UserService özelinde)

📁 Proje Yapısı
MindsetCRM/
├── APIGateway/               --> Ocelot tabanlı servis yönlendirme
├── UserService/             --> Kullanıcı yönetimi (JWT, roller, login/register, loglama)
├── CustomerService/         --> Müşteri yönetimi, filtreleme, not ekleme
├── SalesService/            --> Satış takibi, durum güncelleme, notlar
├── Logging/                 --> Serilog yapılandırmaları ve log servisleri
├── docker-compose.yml       --> Tüm sistemin ayağa kalkması için
└── README.md

🧱 Kullanılan Teknolojiler
ASP.NET Core 8 – Tüm mikroservislerin temel çatısı
C# – Backend dili
MongoDB – NoSQL veritabanı (SalesService için)
SQL Server – UserService ve CustomerService için
JWT Authentication – Kimlik doğrulama
Serilog – Kullanıcı işlemlerinin loglanması (UserService)
Docker & Docker Compose – Servislerin containerize edilmesi
Ocelot API Gateway – Servisler arası yönlendirme ve merkezi giriş noktası
Swagger – API dökümantasyonu
xUnit & Moq – Birim test framework'ü

🛡️ Kimlik Doğrulama Notu
Yalnızca UserService içerisinde kimlik doğrulama ve rol kontrolü uygulanmıştır. Diğer mikroservislerde test süreçlerinin kolay yürütülebilmesi için açık uçlu bırakılmıştır.

🧩 Mikroservisler ve Özellikleri
1. UserService
Login/Register işlemleri
Roller: Admin, Satış Temsilcisi vb.
JWT token üretimi ve doğrulama
Kullanıcı CRUD işlemleri
Serilog ile merkezi loglama altyapısı
Veritabanı: SQL Server

2. CustomerService
Müşteri CRUD işlemleri
Müşteriye not ekleme
Sıralama ve filtreleme (query string destekli)
Veritabanı: SQL Server

3. SalesService
Satış CRUD işlemleri
Durum yönetimi (Yeni, İletişimde, Anlaşma, Kapandı)
Not ve tarih kayıtları
Veritabanı: MongoDB

🧪 Birim Testler
Tüm servisler için xUnit ve Moq kullanılarak testler yazılmıştır.

UserService → Login ve Register testleri
CustomerService → GetAll, Create, Update, Delete, Not Ekleme
SalesService → Create, GetAll, UpdateStatus, AddNote, Delete

📜 API Dokümantasyonu (Swagger)
Her servis Swagger UI üzerinden kendi endpointlerini belgeler:

UserService:        http://localhost:5143/swagger
CustomerService:    http://localhost:5104/swagger
SalesService:       http://localhost:5266/swagger
API Gateway:        http://localhost:9000  üzerinden yönlendirilir(Postmanda test edilebilir.)

🐳 Docker ve Çalıştırma
Tüm sistem aşağıdaki komut ile ayağa kaldırılabilir:
docker-compose up --build
(Swagger üzerinden test veya Postman kullanın)

------------------------------
Docker servisleri:
user-service
customer-service
sales-service
api-gateway
mongo
sql-server

📁 Log Dosyaları
Serilog kullanılarak loglama işlemleri `UserService` içinde otomatik olarak yapılır.
Loglar, container içinden doğrudan host makinedeki `UserService/Logs` klasörüne yazılır.
Log içeriğinde:
- Başarılı kullanıcı kayıtları
- Login işlemleri
- Oluşan hatalar
gibi tüm önemli olaylar detaylı şekilde tutulmaktadır.

🗂 Versiyon Kontrolü ve Teslim
Proje Git ile versiyonlanmış ve GitHub’a yüklenmiştir. Tüm kodlar, testler, Swagger belgeleri ve docker-compose bu repoda mevcuttur.

🛠️ Geliştirici Notu
Bu proje, C# (.NET 8) ile Clean Architecture prensiplerine uygun olarak geliştirilmiştir. Görev yönergesinde Node.js kullanımı artı puan olarak belirtilmiş olsa da, proje tüm fonksiyonel ve mimari gereksinimleri eksiksiz karşılamaktadır. Kurumsal uygulamalarda yaygın olarak kullanılan .NET teknolojisi ile sistemin sürdürülebilirliği, ölçeklenebilirliği ve performansı önceliklendirilmiştir.

Gürkan Kadıoğlu | 02.04.2025
