export const Constant = {
    messageStatus: {
        success: 'success',
        info: 'info',
        warn: 'warn',
        error: 'warn',
    },
    response: {
        isSuccess: 'isSuccess',
        message: 'message',
        data: 'data',
        exception: 'exception',
    },
    auths: {
        isLoginIn: 'loggedIn',
        token: 'token',
        userId: 'userId',
        userName: 'userName',
        currentUser: 'currentUser',
        fullName: 'fullName',
        tokenFirebase: 'tokenFirebase',
        datetimeLogin: 'datetimeLogin',
        expires: 'expires'
    },
    classes: {
        includes: {
            province: {
                country: 'Country',
            },
            district: {
                province: 'Province',
            },
            ward: {
                district: 'District',
                province: 'District.Province',
            },
            Street: {
                province: 'Province',
                district: 'District',
                ward: 'Ward',
                street: 'Street',
            },
            hub: {
                province: 'Province',
                district: 'District',
                ward: 'Ward',
                centerHub: 'CenterHub',
                poHub: 'PoHub',
                pOHub: 'POHub',
            },
            user: {
                department: 'Department',
                role: 'Role',
                hub: 'Hub',
            }
        },
    },
};
