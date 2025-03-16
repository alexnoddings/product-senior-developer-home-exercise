// Trimmed down model based on https://www.rfc-editor.org/rfc/rfc7807
export interface ProblemDetails {
  title: string,
  status: number,
  detail?: string,
  errors?: Record<string, string[]> | undefined;
}
