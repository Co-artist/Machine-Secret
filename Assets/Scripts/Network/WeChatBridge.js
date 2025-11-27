// 微信小游戏桥接代码
// 这个文件需要放在 Unity WebGL 构建的 StreamingAssets 目录中
// 用于 Unity 和微信 API 之间的通信

// Unity 实例引用（由 Unity 自动注入）
var unityInstance = null;

// 设置 Unity 实例
function SetUnityInstance(instance) {
    unityInstance = instance;
}

// 微信登录
function WeChatLogin() {
    wx.login({
        success: function(res) {
            if (res.code) {
                // 发送 code 到 Unity
                if (unityInstance) {
                    unityInstance.SendMessage('NetworkManager', 'OnWeChatLogin', res.code);
                }
            } else {
                console.error('微信登录失败:', res.errMsg);
                if (unityInstance) {
                    unityInstance.SendMessage('NetworkManager', 'OnWeChatLoginFailed', res.errMsg);
                }
            }
        },
        fail: function(err) {
            console.error('微信登录错误:', err);
            if (unityInstance) {
                unityInstance.SendMessage('NetworkManager', 'OnWeChatLoginFailed', JSON.stringify(err));
            }
        }
    });
}

// 获取用户信息
function GetUserInfo() {
    wx.getUserInfo({
        success: function(res) {
            if (unityInstance) {
                unityInstance.SendMessage('NetworkManager', 'OnGetUserInfo', JSON.stringify(res.userInfo));
            }
        },
        fail: function(err) {
            console.error('获取用户信息失败:', err);
        }
    });
}

// 显示分享菜单
function ShowShareMenu() {
    wx.showShareMenu({
        withShareTicket: true,
        success: function() {
            console.log('分享菜单已显示');
        }
    });
}

// 设置分享内容
function SetShareAppMessage(title, path, imageUrl) {
    wx.onShareAppMessage(function() {
        return {
            title: title,
            path: path,
            imageUrl: imageUrl || ''
        };
    });
}

// 分享到好友
function ShareAppMessage(title, path, imageUrl) {
    wx.shareAppMessage({
        title: title,
        path: path,
        imageUrl: imageUrl || '',
        success: function() {
            if (unityInstance) {
                unityInstance.SendMessage('NetworkManager', 'OnShareSuccess', '');
            }
        },
        fail: function(err) {
            console.error('分享失败:', err);
            if (unityInstance) {
                unityInstance.SendMessage('NetworkManager', 'OnShareFailed', JSON.stringify(err));
            }
        }
    });
}

// 获取好友排行榜
function GetFriendCloudStorage(keyList) {
    wx.getFriendCloudStorage({
        keyList: keyList,
        success: function(res) {
            if (unityInstance) {
                unityInstance.SendMessage('NetworkManager', 'OnGetFriendCloudStorage', JSON.stringify(res.data));
            }
        },
        fail: function(err) {
            console.error('获取好友排行榜失败:', err);
            if (unityInstance) {
                unityInstance.SendMessage('NetworkManager', 'OnGetFriendCloudStorageFailed', JSON.stringify(err));
            }
        }
    });
}

// 设置用户排行榜数据
function SetUserCloudStorage(keyValueList) {
    wx.setUserCloudStorage({
        KVDataList: keyValueList,
        success: function() {
            console.log('用户数据已保存');
        },
        fail: function(err) {
            console.error('保存用户数据失败:', err);
        }
    });
}

// 显示提示
function ShowToast(title, icon, duration) {
    wx.showToast({
        title: title,
        icon: icon || 'none',
        duration: duration || 2000
    });
}

// 显示加载提示
function ShowLoading(title) {
    wx.showLoading({
        title: title || '加载中...',
        mask: true
    });
}

// 隐藏加载提示
function HideLoading() {
    wx.hideLoading();
}

// 显示模态对话框
function ShowModal(title, content) {
    wx.showModal({
        title: title,
        content: content,
        showCancel: true,
        success: function(res) {
            if (unityInstance) {
                unityInstance.SendMessage('NetworkManager', 'OnModalResult', res.confirm ? 'confirm' : 'cancel');
            }
        }
    });
}

// 导出函数供 Unity 调用
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        SetUnityInstance: SetUnityInstance,
        WeChatLogin: WeChatLogin,
        GetUserInfo: GetUserInfo,
        ShowShareMenu: ShowShareMenu,
        SetShareAppMessage: SetShareAppMessage,
        ShareAppMessage: ShareAppMessage,
        GetFriendCloudStorage: GetFriendCloudStorage,
        SetUserCloudStorage: SetUserCloudStorage,
        ShowToast: ShowToast,
        ShowLoading: ShowLoading,
        HideLoading: HideLoading,
        ShowModal: ShowModal
    };
}

