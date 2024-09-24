import { LogLevel, Configuration, BrowserCacheLocation } from '@azure/msal-browser';

const isIE = window.navigator.userAgent.indexOf("MSIE ") > -1 || window.navigator.userAgent.indexOf("Trident/") > -1;

export const b2cPolicies = {
  names: {
    signUpSignIn: "B2C_1_Sigup_SignIn",
    editProfile: "B2C_1_EditProfile" // todo create and update policy name


  },
  authorities: {
    signUpSignIn: {
      authority: "https://inventoryorg.b2clogin.com/inventoryorg.onmicrosoft.com/B2C_1_Sigup_SignIn"
    },
    editProfile: {
      authority: "https://inventoryorg.b2clogin.com/inventoryorg.onmicrosoft.com/B2C_1_EditProfile"
    }
  },
  authorityDomain: "inventoryorg.b2clogin.com"
};


export const msalConfig: Configuration = {
  auth: {
    clientId: '<your-MyApp-application-ID>',
    authority: b2cPolicies.authorities.signUpSignIn.authority,
    knownAuthorities: [b2cPolicies.authorityDomain],
    redirectUri: '/',
  },
  cache: {
    cacheLocation: BrowserCacheLocation.LocalStorage,
    storeAuthStateInCookie: isIE,
  },
  system: {
    loggerOptions: {
      loggerCallback: (logLevel, message, containsPii) => {
        console.log(message);
      },
      logLevel: LogLevel.Verbose,
      piiLoggingEnabled: false
    }
  }
}

export const protectedResources = {
  todoListApi: {
    endpoint: "http://localhost:5000/api/todolist",
    scopes: ["https://inventoryorg.onmicrosoft.com/api/poducts.read"],
  },
}
export const loginRequest = {
  scopes: []
};
