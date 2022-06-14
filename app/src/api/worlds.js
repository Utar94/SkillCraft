import { _delete, get, post, put } from '.'

export async function createWorld({ alias, description, name }) {
  return await post('/worlds', { alias, description, name })
}

export async function deleteWorld(id) {
  return await _delete(`/worlds/${id}`)
}

export async function getWorld(id) {
  return await get(`/worlds/${id}`)
}

export async function getWorlds({ search, sort, desc, index, count }) {
  const query = Object.entries({ search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/worlds?${query}`)
}

export async function updateWorld(id, { description, name }) {
  return await put(`/worlds/${id}`, { description, name })
}
