import { Routes } from '@angular/router';
import { DocumentViewer } from './document-viewer/document-viewer';
import { VerificationFailed } from './verification-failed/verification-failed';
import { ResetPassword } from './reset-password/reset-password';

export const routes: Routes = [
    {
        path:'',
        loadComponent: () =>
            import('./main-layout-component/main-layout-component').then(m => m.MainLayoutComponent)
    },
    {
        path:'login',
        loadComponent:() =>
            import('./login/login').then(m => m.Login)
    },
    {
        path:'register',
        loadComponent: () => 
            import('./register/register').then(m => m.Register)
    },
    {
        path:'features',
        loadComponent: () => 
            import('./features/features').then(m => m.Features)
    },
    {
        path:'dashboard',
        loadComponent: () =>
            import('./dashboard/dashboard').then(m => m.Dashboard)
    },
    {
        path:'about',
        loadComponent: () => 
            import('./about/about').then(m => m.About)
    },
    {
        path:'contact',
        loadComponent: () => 
            import('./contact/contact').then(m => m.Contact)
    },
    {
        path:'blog',
        loadComponent: () => 
            import('./blog/blog').then(m => m.Blog)
    },
    {
        path:'reset-password',
        loadComponent:() => 
            import('./reset-password/reset-password').then(m => m.ResetPassword)
    },
    {
        path:'verify-account',
        loadComponent: () =>
            import('./verify-account/verify-account').then(v => v.VerifyAccount)
    },
    {
        path:'documents',
        loadComponent: () => 
            import('./documents/documents').then(d => d.Documents)
    },
    {
        path:'documents/:folderId',
        loadComponent: () =>
                import('./documents/documents').then(d => d.Documents)
    },
    {
        path:'document-editor/:id',
        loadComponent:() =>
            import('./document-viewer/document-viewer').then(d => DocumentViewer)
    },
    {
        path:'verification-failed',
        loadComponent:()=>
            import('./verification-failed/verification-failed').then(v => v.VerificationFailed)
    }
];
