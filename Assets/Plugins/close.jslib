mergeInto(LibraryManager.library, 
{
      closewindow: function (RedirectUrl)
	{
	console.log('Encoded Url Received');
	console.log(RedirectUrl);
        console.log('Url Reciedved Decoded');
        console.log(UTF8ToString(RedirectUrl));
        redirect_to = UTF8ToString(RedirectUrl);
        console.log(redirect_to);
        window.parent.location.href = redirect_to
}
});