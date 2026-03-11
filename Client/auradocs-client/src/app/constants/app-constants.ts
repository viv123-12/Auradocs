
export enum INPUTTYPES
{
    TEXT = "text",
    PASSWORD = "password"
}

export enum DOCUMENT_TYPES
{
    ALL  = 0,
    PDF = 1,
    DOC = 2
}

export enum DOCUMENT_STATUS
{
  ALL = 0,
  PUBLISHED = 1,
  DRAFT = 2
}

export enum SHARE_SCOPE {
  ALL = 0,
  ME = 1,
  TEAM = 2
}

export interface DropDownOptions<T>
{
  label:string,
  value:T
}

export const DOCUMENT_STATUS_FILTER_OPTIONS: DropDownOptions<DOCUMENT_STATUS>[] = [
  { label: 'All', value: DOCUMENT_STATUS.ALL },
  { label: 'Approved', value: DOCUMENT_STATUS.PUBLISHED },
  { label: 'Draft', value: DOCUMENT_STATUS.DRAFT }
]

export const DOCUMENT_TYPES_FILTER_OPTIONS: DropDownOptions<DOCUMENT_TYPES>[] = [
  { label: 'All', value: DOCUMENT_TYPES.ALL },
  { label: 'Pdf', value: DOCUMENT_TYPES.PDF },
  { label: 'Doc', value: DOCUMENT_TYPES.DOC }
]

export const DOCUMENT_SHARE_SCOPE_FILTER_OPTIONS: DropDownOptions<SHARE_SCOPE>[] = [
  { label: 'All', value: SHARE_SCOPE.ALL },
  { label: 'Me', value: SHARE_SCOPE.ME },
  { label: 'Team', value: SHARE_SCOPE.TEAM }
]

export const VERIFICATION_FAILURE_RESULTS:string[] = [
  "The verification link has expired",
  "The link was already used",
  "The link is invalid or tampered with"
];


