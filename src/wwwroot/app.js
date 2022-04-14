document.addEventListener('DOMContentLoaded', function() {
  var address = document.querySelector('input[name=address]');
  var connect = document.querySelector('input[name=connect]');
  address.addEventListener('keydown', x => x.keyCode === 13
    ? connect.click()
    : undefined);
  connect.addEventListener('click', () => address.value
    ? location.href = `/${encodeURIComponent(address.value)}/`
    : undefined);
});
