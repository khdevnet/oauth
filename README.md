## OAuth and OpenId
[OAuth 2.0](https://www.okta.com/identity-101/whats-the-difference-between-oauth-openid-connect-and-saml/) is a framework that controls authorization to a protected resource such as an application or a set of files, while OpenID Connect is industry standards for[Federated authentication](https://www.okta.com/identity-101/what-is-federated-identity/).

* OAuth 2.0: If you’ve ever signed up to a new application and agreed to let it automatically source new contacts via Facebook or your phone contacts, then you’ve likely used OAuth 2.0. This standard provides secure **delegated access**. That means an application can take actions or access resources from a server on behalf of the user, without them having to share their credentials. It does this by allowing the identity provider (IdP) to issue tokens to third-party applications with the user’s approval.

* OpenID Connect: If you’ve used your Google to sign in to applications like YouTube, or Facebook to log into an online shopping cart, then you’re familiar with this authentication option. OpenID Connect is an open standard that organizations use to authenticate users. IdPs use this so that users can sign in to the IdP, and then access other websites and apps without having to log in or share their sign-in information. 

## JWT vs Id Token vs Access token
[JSON Web Token (JWT)](https://jwt.io/introduction) is an open standard (RFC 7519) that defines a compact and self-contained way for securely transmitting information between parties as a JSON object. This information can be verified and trusted because it is digitally signed.

[ID token](https://www.oauth.com/oauth2-servers/openid-connect/id-tokens/) contains information about what happened when a user authenticated, and is intended to be read by the OAuth client. 
The ID token may also contain information about the user such as their name or email address, although that is not a requirement of an ID token.

[Access tokens](https://oauth.net/2/access-tokens/) are what the OAuth client uses to make requests to an API.
The access token is meant to be read and validated by the API. 

Here are some further differences between ID tokens and access tokens:
* ID tokens are meant to be read by the OAuth client. Access tokens are meant to be read by the resource server.
* ID tokens are JWTs. Access tokens can be JWTs but may also be a random string.
* ID tokens should never be sent to an API. Access tokens should never be read by the client.

[Id token vs Access token](https://oauth.net/id-tokens-vs-access-tokens/)
[Difference](https://auth0.com/blog/id-token-access-token-what-is-the-difference/)
[optimize id tokens](https://leastprivilege.com/2016/12/14/optimizing-identity-tokens-for-size/)
