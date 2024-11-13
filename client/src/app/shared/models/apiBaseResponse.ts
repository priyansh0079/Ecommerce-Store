export type ApiBaseResponse<T> = {
    statusCode: number;
    message: string;
    content: T;
}