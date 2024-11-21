
        let tabCount = 0;
        const MAX_TABS = 10; // 最大标签页数量
        let activeTab = null; // 当前激活的标签
        const tabContents = {}; // 存储每个标签的内容
        document.getElementById('newTab').addEventListener('click', function() {
            if (tabCount < MAX_TABS) {
                let tabName;
                let isTabNameTaken = true;
                do {
                    tabName = prompt("请输入标签页的名称：");
                    if (tabName === null) return; // 用户取消
                    tabName = tabName.trim(); // 去除前后空格
                    if(tabName ==="")
                    {
                        alert("请不要输入空白");
                        continue;
                    }
                    if (isTabNameExists(tabName)) {
                        isTabNameTaken = true;
                        alert("标签名已存在，请输入一个唯一的名称。");
                    }
                    else
                    {
                        isTabNameTaken = false;
                    }
                } while (isTabNameTaken); // 检查标签是否已存在

                $.ajax({
                    url: '/Record/CreateTabTable',
                    type: 'POST',
                    data: tabName,
                    success: function(response) {
                        // 处理成功响应
                        console.log('Tab created ', response.message);
                        if(response.success == true)
                        {                
                            tabCount++;

                            // 创建新的标签
                            const tab = document.createElement('div');
                            tab.className = 'tab';
                            tab.textContent = tabName; // 使用用户输入的名称
                            tab.addEventListener('click', function() {
                                activateTab(tab);
                            });
                            
                            // 初始化该标签的内容区域
                            tabContents[tabName] = [];
                            
                            // 将标签添加到标签栏
                            document.getElementById('tabs').appendChild(tab);
                            activateTab(tab); // 激活新标签}
                        }
                        else if(response.success == false)
                        {
                            alert("数据库tab创建失败");
                        }
                    },
                    error: function(error) {
                        console.error('Error:', error);
                        alert("数据库tab创建失败,请检查网络");
                        // 处理错误响应
                    }
                })
            } else {
                alert('最多只能创建 ' + MAX_TABS + ' 个标签页。');
            }
        });

        document.getElementById('deleteTab').addEventListener('click', function() {
            if (activeTab) {
                const confirmDelete = confirm('您确定要删除当前标签页吗？');
                if (confirmDelete) {
                    // 移除当前激活的标签
                    $.ajax({
                        url: '/Record/DeleteTabTable',
                        type: 'DELETE',
                        data: tabName,
                        success: function(response) {
                            if(response.success == true)
                            {
                                const tabName = activeTab.textContent;
                                activeTab.remove();
                                tabCount--;
                                activeTab = null; // 清空当前激活标签
                                document.getElementById('content').textContent = ''; // 清空内容区域
                                delete tabContents[tabName]; // 删除该标签的内容
                            }
                            else if(response.success == false)
                            {
                                alert("数据库tab删除失败");
                            }
                        }
                        ,error: function(error) {
                            console.error('Error:', error);
                            alert("数据库tab删除失败,请检查网络");
                            // 处理错误响应
                        }
                    })
                    
                }
            } else {
                alert('没有选择任何标签页。');
            }
        });

        document.getElementById('addContent').addEventListener('click', function() {
            if (activeTab) {
                // 显示模态窗口
                document.getElementById('modal').style.display = 'flex';
            } else {
                alert('没有选择任何标签页。');
            }
        });

        document.getElementById('image').addEventListener('change', function(event) {
            const file = event.target.files[0];
            const maxSize = 20 * 1024 * 1024; // 最大文件大小设为 20 MB
            if (file) {
                // 检查文件大小
                if (file.size > maxSize) {
                    alert('文件大小超过限制（最大 20 MB）。');
                    label.innerText = last_name;
                    return; // 结束函数
                } 
                const reader = new FileReader();
                reader.onload = function(e) {
                    const imgPreview = document.getElementById('imagePreview');
                    imgPreview.src = e.target.result;
                    imgPreview.style.display = 'block'; // 显示预览
                };
                reader.readAsDataURL(file);

            }
            else
            {

                const imgPreview = document.getElementById('imagePreview');
                imgPreview.src = "";
                imgPreview.style.display = 'none';
            }
        });

        document.getElementById('saveContent').addEventListener('click', function() {
            const dateValue = document.getElementById('date').value;
            const imageUrl = document.getElementById('imagePreview').src;
            const description = document.getElementById('description').value;
            const messageElement = document.getElementById('message');
            messageElement.style.display = 'none'; // 隐藏消息

            if (!dateValue || !imageUrl || !description) {
                messageElement.textContent = '请填写所有字段。';
                messageElement.style.color = 'red';
                messageElement.style.display = 'block';
                return;
            }

            const tabName = activeTab.textContent;
            const contentBlock = {
                id : null,
                tabName : tabName,
                dateValue: new Date(dateValue),
                imageUrl: imageUrl,
                description: description
            };

            $.ajax({
                url: '/Record/InsertTabData',
                type: 'POST',
                data: JSON.stringify(contentBlock),
                contentType: 'application/json',
                success: function(response) {
                    if(response.success == true)
                    {
                        // 添加到该标签的内容列表
                        contentBlock.id = response.id;
                        tabContents[tabName].push(contentBlock);
            
                        // 更新内容区域
                        updateContentArea(tabName);
            
                        // 隐藏模态窗口
                        document.getElementById('modal').style.display = 'none';
            
                        // 清空输入
                        document.getElementById('date').value = '';
                        document.getElementById('image').value = '';
                        document.getElementById('imagePreview').style.display = 'none';
                        document.getElementById('description').value = '';
            
                        // 显示成功消息
                        messageElement.textContent = '内容块添加成功！';
                        messageElement.style.color = 'green';
                        messageElement.style.display = 'block';
                    }
                    else if(response.success == false)
                    {
                        alert("数据库添加失败");
                    }
                }
                ,error: function(error) {
                    console.error('Error:', error);
                    alert("数据库添加失败,请检查网络");
                    // 处理错误响应
                }
            })
            // 添加到该标签的内容列表

        });

        document.getElementById('cancel').addEventListener('click', function() {
            // 隐藏模态窗口
            document.getElementById('modal').style.display = 'none';
            // 清空输入
            document.getElementById('date').value = '';
            document.getElementById('image').value = '';
            document.getElementById('imagePreview').style.display = 'none';
            document.getElementById('description').value = '';
        });

        document.getElementById('content-delete-button').addEventListener('click', function(event) {
            if (activeTab) {
                const tabName = activeTab.textContent;
                const selectedBlocks = event.target.closest('.content-block');
                const childIdelements = selectedBlocks.querySelector('id');
                if (selectedBlocks) {
                    // 确认删除
                    const confirmDelete = confirm('您确定要删除选中的内容块吗？');
                    if (confirmDelete) {
                            $.ajax({
                                url: '/Record/DeleteTabData',
                                type: 'DELETE',
                                data: JSON.stringify({
                                    tabName: tabName,
                                    id : childIdelements.innerText,
                                }),
                                contentType: 'application/json',
                                success: function(response) {
                                    if(response.success == true)
                                    {
                                       var index =  tabContents[tabName].findIndex(block => block.id === childIdelements.innerText);
                                       if (index !== -1) 
                                        {
                                           tabContents[tabName].splice(index, 1);
                                           selectedBlocks.innerHTML = '';
                                        }
                                       else
                                       {
                                            alert('数组中未找到该ID,但是数据库删掉了');
                                       }
                                    }
                                    else if(response.success == false)
                                    {
                                        alert("数据库删除失败");
                                    }
                                }
                                ,error: function(error) {
                                    console.error('Error:', error);
                                    alert("数据库删除失败,请检查网络");
                                    // 处理错误响应
                                }
                            });
                    }
                }
                else 
                {
                    alert('没有找到要删除的内容块。');
                }
            } 
            else {
                alert('没有选择任何标签页。');
            }
            updateContentArea(activeTab.textContent);
        });

        function activateTab(tab) {
            // 移除其他标签的激活状态
            const tabs = document.querySelectorAll('.tab');
            tabs.forEach(t => t.classList.remove('active'));

            // 激活当前标签
            tab.classList.add('active');
            activeTab = tab; // 更新当前激活标签

            // 更新内容区域
            const tabName = tab.textContent;
            updateContentArea(tabName);
        }

        function updateContentArea(tabName) {
            const contentArea = document.getElementById('content');
            contentArea.innerHTML = ''; // 清空现有内容
            // 获取该标签的内容并显示
            const contents = tabContents[tabName];
            if (Array.isArray(contents) && contents.length > 0) {
                contents.forEach((content, index) => {
                    const block = document.createElement('div');
                    block.className = 'content-block';
                    block.innerHTML = `
                        <div class="id hidden">${content.id}></div>
                        <div class="time">${content.dateValue.toLocaleDateString("zh-CN")}</div>
                        <img class="preview" src="${content.imageUrl}" alt="Image Preview"> </img>
                        <div class="description">${content.description}</div>
                        <button class="content-delete-button" data-index="${index}">删除</button>
                    `;
                    contentArea.appendChild(block);})
                    
                    console.log("更新成功!");
                    console.log(contents.length);
            } else {
                contentArea.textContent = '该标签没有内容。';
                console.log("更新失败");
            }
        }
        function isTabNameExists(name) {
            const tabs = document.querySelectorAll('#tabs .tab'); // 获取所有标签
            for (let tab of tabs) {
                if (tab.textContent === name) {
                    return true; // 找到重名的标签
                }
            }
            return false; // 没有重名的标签
        }

        function pageInit() {
            
        }

