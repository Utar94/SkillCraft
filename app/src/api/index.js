import router from '@/router'
import store from '@/store'

const baseUrl = process.env.VUE_APP_API_BASE_URL
const contentType = 'Content-Type'

async function executeWithRenew(method, url, data = null) {
  let token = store.state.token
  let world = store.state.world
  try {
    return await execute(method, url, data, token, world)
  } catch (e) {
    const { status } = e
    if (status === 401 && token) {
      const { refresh_token } = token
      if (refresh_token) {
        try {
          token = (await execute('POST', '/identity/renew', { refresh_token })).data
          store.dispatch('signIn', token)
          if (localStorage.getItem(process.env.VUE_APP_TOKEN_STORAGE_KEY)) {
            localStorage.setItem(process.env.VUE_APP_TOKEN_STORAGE_KEY, JSON.stringify(token))
          }
          return await execute(method, url, data, token, world)
        } catch (e) {
          console.error(e)
          store.dispatch('signOut')
          localStorage.removeItem(process.env.VUE_APP_TOKEN_STORAGE_KEY)
          router.push({ name: 'SignIn' })
        }
      }
    }
    throw e
  }
}

async function execute(method, url, data = null, token = null, world = null) {
  const request = {
    headers: {},
    method
  }
  if (data) {
    request.body = JSON.stringify(data)
    request.headers[contentType] = 'application/json; charset=UTF-8'
  }
  if (token) {
    const { access_token, token_type } = token
    request.headers['Authorization'] = `${token_type} ${access_token}`
  }
  if (world) {
    request.headers['World'] = world.alias
  }
  const response = await fetch(`${baseUrl}${url}`, request)
  const result = { data: null, status: response.status }
  const dataType = response.headers.get(contentType)
  if (dataType) {
    if (dataType.includes('json')) {
      result.data = await response.json()
    } else {
      throw new Error(`The content type "${dataType}" is not supported.`)
    }
  }
  if (!response.ok) {
    throw result
  }
  return result
}

export async function _delete(url) {
  return await executeWithRenew('DELETE', url)
}

export async function get(url) {
  return await executeWithRenew('GET', url)
}

export async function patch(url, data = null) {
  return await executeWithRenew('PATCH', url, data)
}

export async function post(url, data = null) {
  return await executeWithRenew('POST', url, data)
}

export async function put(url, data = null) {
  return await executeWithRenew('PUT', url, data)
}
