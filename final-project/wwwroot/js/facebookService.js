$.getScript('https://connect.facebook.net/en_US/sdk.js', function () {
    FB.init({
        appId: '2411340119110577', //app secret 9e97d8d101a13b4bf0ba306d53485b7e
        version: 'v2.7' // or v2.1, v2.2, v2.3, ...
    });

    $("#fb-share-button").on('click', function () {
        FB.ui({
            method: 'share',
            href: 'https://www.facebook.com/SweetShop-1043212635885066/?modal=admin_todo_tour',
        }, function (response) { });
    })
});