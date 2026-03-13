import { Routes } from '@angular/router';
import { DocumentViewer } from './Pages/document-viewer/document-viewer';

export const routes: Routes = [
    {
        path:'',
        loadComponent: () =>
            import('./Pages/main-layout-component/main-layout-component').then(m => m.MainLayoutComponent)
    },
    {
        path:'login',
        loadComponent:() =>
            import('./Pages/login/login').then(m => m.Login)
    },
    {
        path:'register',
        loadComponent: () => 
            import('./Pages/register/register').then(m => m.Register)
    },
    {
        path:'features',
        loadComponent: () => 
            import('./Pages/features/features').then(m => m.Features)
    },
    {
        path:'dashboard',
        loadComponent: () =>
            import('./Pages/dashboard/dashboard').then(m => m.Dashboard)
    },
    {
        path:'about',
        loadComponent: () => 
            import('./Pages/about/about').then(m => m.About)
    },
    {
        path:'contact',
        loadComponent: () => 
            import('./Pages/contact/contact').then(m => m.Contact)
    },
    {
        path:'blog',
        loadComponent: () => 
            import('./blog/blog').then(m => m.Blog)
    },
    {
        path:'reset-password',
        loadComponent:() => 
            import('./Pages/reset-password/reset-password').then(m => m.ResetPassword)
    },
    {
        path:'verify-account',
        loadComponent: () =>
            import('./verify-account/verify-account').then(v => v.VerifyAccount)
    },
    {
        path:'documents',
        loadComponent: () => 
            import('./Pages/documents/documents').then(d => d.Documents)
    },
    {
        path:'documents/:folderId',
        loadComponent: () =>
                import('./Pages/documents/documents').then(d => d.Documents)
    },
    {
        path:'document-editor/:id',
        loadComponent:() =>
            import('./Pages/document-viewer/document-viewer').then(d => DocumentViewer)
    },
    {
        path:'verification-failed',
        loadComponent:()=>
            import('./verification-failed/verification-failed').then(v => v.VerificationFailed)
    }
];
