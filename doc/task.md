总任务：制作一个挂在云服务器上的，能体现合格工作技术的展示网页
实用技术：IIS C# MVC RAZOR HTML CSS JAVASCRIPT MYSQL VSCODE restful-api

内容：
1、输入对应Ip地址+端口号呈现出用户登陆界面，可联系我添加账户密码，只有输入账号密码才能进入访问接下来的内容（用户信息账户密码保存到数据库中）
2、登陆后出现首页为导航页，分别为个人简历信息，日常生活记录（金豆-按日期排序，使用数据库)，当日信息数据（为天气，财经等api数据显示）
3、将网页部署在腾讯云上面，并将整个项目代码上传github

登陆界面：仅有账号密码，以及本人联系方式，以及登录按钮，初期所有账号等级一致
拓展：可将账号分为管理员等级与游客等级，每个等级的用户将看到不同的内容以及拥有不同的权力

首页：中心排布三大板块，点击任一板块跳转到对应网址，除了首页以及登陆界面以外，所有界面上方出现三个板块外加首页的板块按钮，
点击可达各个模块网页 ，以及退出登录按钮，点击可以前往登陆界面切换账号

个人简历：内涵各种个人信息，邮箱和github地址有对应链接可以点击，也可附带个人社交媒体账号外链
拓展：可将各个经历标题连接到生活记录各个板块

生活记录：目前只增加金豆板块，记录金豆的各个阶段，板块中拥有图片和简介两大数据，按时间排列，拥有添加删除按钮，可以增加删除各个时间点，
以及添加照片简介，添加进去的内容将根据时间自动排列排版
拓展：可根据不同的账号等级展示不同内容，以及拥有不同权限

信息数据：功能性板块，赋予该网站功能性，提纯互联网信息，初期功能为查看天气，查看金融
拓展：可添加各种不同信息窗口，添加物联网控制与监控板块等

服务器：限制账号登陆数量，请求次数等
