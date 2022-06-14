import { isLetterOrDigit } from '@/helpers/stringUtils'

export default function (value) {
  return value.split('-').every(word => word.length && word.split('').every(isLetterOrDigit))
}
