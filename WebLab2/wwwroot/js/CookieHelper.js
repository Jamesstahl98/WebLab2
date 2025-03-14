window.setCookie = (name, value, expirationDays = 1) => {
    const date = new Date();
    date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    const expires = "expires=" + date.toUTCString();

    const cookieString = `${name}=${value};${expires};path=/;secure;samesite=strict;`;
    document.cookie = cookieString;

    console.log(`Cookie set: ${cookieString}`);
};