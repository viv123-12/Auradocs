const LOCAL_ENVIRONEMNT = "http://localhost:5170";
export const API_CONSTANTS =
{
    AUTH:{
        REGISTER_USER: `${LOCAL_ENVIRONEMNT}/User/register-user`,
        LOGIN: `${LOCAL_ENVIRONEMNT}/User/login`,
        LOGOUT: `${LOCAL_ENVIRONEMNT}/User/logout`,
        RESET_PASSWORD: `${LOCAL_ENVIRONEMNT}/User/reset-password`,
        VERIFY_ACCOUNT: `${LOCAL_ENVIRONEMNT}/User/verify-account`
    },
    CHECK_LOGIN:{
        CHECK_LOGIN:`${LOCAL_ENVIRONEMNT}/CheckLoginState/check-login-state`
    },
    MASTER_DATA_SERVICE: {
        DOMAIN_DROPDOWN : `${LOCAL_ENVIRONEMNT}/MasterData/get-domainname-dropdown`,
        DOMAIN_PRACTICE_AREA_DROPDOWN : `${LOCAL_ENVIRONEMNT}/MasterData/get-domainPracticearea-dropdown`
    },
    DOCUMENTS:{
        GET_FOLDERS: `${LOCAL_ENVIRONEMNT}/DocumentManager/folders`,
        GET_FILES: `${LOCAL_ENVIRONEMNT}/DocumentManager/documents`,
        CREATE_DOCUMENTS: `${LOCAL_ENVIRONEMNT}/DocumentManager/document`,
        CREATE_FOLDER: `${LOCAL_ENVIRONEMNT}/DocumentManager/folder`,
        UPDATE_DOCUMENT: `${LOCAL_ENVIRONEMNT}/DocumentManager/document`,
        DUPLICATE_DOCUMENT:`${LOCAL_ENVIRONEMNT}/DocumentManager/duplicate-document`
    }
}